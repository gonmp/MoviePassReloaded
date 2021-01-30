using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public class ShowsService : IShowsService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public ShowsService(DataAccessContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<Show>> GetAllAsync()
        {
            var shows = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .ToListAsync();

            return _mapper.Map<List<Show>>(shows);
        }

        public async Task<Show> GetAsync(int id)
        {
            var show = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(s => s.Id == id);

            return _mapper.Map<Show>(show);
        }

        public async Task<Show> SaveAsync(Show show)
        {
            var mappedShow = _mapper.Map<DataAccess.EntityModels.Show>(show);

            var movie = await _dataContext.Movies
                .Include(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .SingleOrDefaultAsync(m => m.Id == show.MovieId);
            if (movie == null)
            {
                throw new System.ArgumentException("There is no movie with Id = " + movie.Id, "movie.Id");
            }

            var room = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(r => r.Id == show.RoomId);
            if (room == null)
            {
                throw new System.ArgumentException("There is no room with Id = " + room.Id, "room.Id");
            }

            mappedShow.Movie = movie;
            mappedShow.Room = room;

            await _dataContext.Shows.AddAsync(mappedShow);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Show>(mappedShow);
        }

        public async Task<Show> UpdateAsync(Show show)
        {
            var result = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(s => s.Id == show.Id);
            if (result == null)
            {
                throw new System.ArgumentException("There is no show with Id = " + show.Id, "show.Id");
            }

            var movie = await _dataContext.Movies
                .Include(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .SingleOrDefaultAsync(m => m.Id == show.MovieId);
            if (movie == null)
            {
                throw new System.ArgumentException("There is no movie with Id = " + movie.Id, "movie.Id");
            }

            var room = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(r => r.Id == show.RoomId);
            if (room == null)
            {
                throw new System.ArgumentException("There is no room with Id = " + room.Id, "room.Id");
            }

            result.Movie = movie;
            result.Room = room;
            result.DateTime = show.DateTime;

            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Show>(result);
        }

        public async Task<Room> DeleteAsync(int id)
        {
            var showToDelete = await _dataContext.Shows.SingleOrDefaultAsync(s => s.Id == id);

            if (showToDelete == null) return null;

            _dataContext.Remove(showToDelete);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Room>(showToDelete);
        }
    }
}
