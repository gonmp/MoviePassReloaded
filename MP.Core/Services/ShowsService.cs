using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Helpers;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Core.Response;
using MP.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ServiceResponse<List<Show>>> GetAllAsync()
        {
            var shows = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.Genres)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .ToListAsync();

            var mappesShowsList = _mapper.Map<List<Show>>(shows);

            return new ServiceResponse<List<Show>>(mappesShowsList);
        }

        public async Task<ServiceResponse<Show>> GetAsync(int id)
        {
            var show = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.Genres)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (show == null)
                return new ServiceResponse<Show>(new Error(ErrorCodes.ShowNotExists, ErrorMessages.ShowNotExists(id)));

            var mappedShow = _mapper.Map<Show>(show);

            return new ServiceResponse<Show>(mappedShow);
        }

        public async Task<ServiceResponse<Show>> SaveAsync(Show show)
        {
            var mappedShow = _mapper.Map<DataAccess.EntityModels.Show>(show);

            var movie = await _dataContext.Movies
                .Include(m => m.Genres)
                .SingleOrDefaultAsync(m => m.Id == show.MovieId);

            if (movie == null)
                return new ServiceResponse<Show>(new Error(ErrorCodes.MovieNotExists, ErrorMessages.MovieNotExists(show.MovieId)));

            var room = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(r => r.Id == show.RoomId);

            if (room == null)
                return new ServiceResponse<Show>(new Error(ErrorCodes.RoomNotExists, ErrorMessages.RoomNotExists(show.RoomId)));

            mappedShow.Movie = movie;
            mappedShow.Room = room;

            await _dataContext.Shows.AddAsync(mappedShow);
            await _dataContext.SaveChangesAsync();

            var savedShow = _mapper.Map<Show>(mappedShow);

            return new ServiceResponse<Show>(savedShow);
        }

        public async Task<ServiceResponse<Show>> UpdateAsync(Show show)
        {
            var result = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.Genres)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(s => s.Id == show.Id);

            if (result == null)
                return new ServiceResponse<Show>(new Error(ErrorCodes.ShowNotExists, ErrorMessages.ShowNotExists(show.Id)));

            var movie = await _dataContext.Movies
                .Include(m => m.Genres)
                .SingleOrDefaultAsync(m => m.Id == show.MovieId);

            if (movie == null)
                return new ServiceResponse<Show>(new Error(ErrorCodes.MovieNotExists, ErrorMessages.MovieNotExists(show.MovieId)));

            var room = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(r => r.Id == show.RoomId);

            if (room == null)
                return new ServiceResponse<Show>(new Error(ErrorCodes.RoomNotExists, ErrorMessages.RoomNotExists(show.RoomId)));

            result.Movie = movie;
            result.Room = room;
            result.DateTime = show.DateTime;

            await _dataContext.SaveChangesAsync();

            var updatedShow = _mapper.Map<Show>(result);

            return new ServiceResponse<Show>(updatedShow);
        }

        public async Task<ServiceResponse<Show>> DeleteAsync(int id)
        {
            var showToDelete = await _dataContext.Shows.SingleOrDefaultAsync(s => s.Id == id);

            if (showToDelete == null)
                return new ServiceResponse<Show>(new Error(ErrorCodes.ShowNotExists, ErrorMessages.ShowNotExists(id)));

            _dataContext.Remove(showToDelete);
            await _dataContext.SaveChangesAsync();

            var deletedShow = _mapper.Map<Show>(showToDelete);

            return new ServiceResponse<Show>(deletedShow);
        }

        public async Task<ServiceResponse<List<Show>>> GetShowsListingsAsync()
        {
            var nextThursday = Dates.GetNextDayOfWeek((int)DayOfWeek.Thursday);
            var today = DateTime.Now;

            var shows = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.Genres)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Where(s => s.DateTime >= today && s.DateTime <= nextThursday)
                .ToListAsync();

            var mappesShowsList = _mapper.Map<List<Show>>(shows);

            return new ServiceResponse<List<Show>>(mappesShowsList);
        }

        public async Task<ServiceResponse<List<Show>>> GetShowsListingsAsync(int cinemaId)
        {
            var nextThursday = Dates.GetNextDayOfWeek((int)DayOfWeek.Thursday);
            var today = DateTime.Now;

            var shows = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.Genres)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Where(s => s.DateTime >= today && s.DateTime <= nextThursday && s.Room.CinemaId == cinemaId)
                .ToListAsync();

            var mappesShowsList = _mapper.Map<List<Show>>(shows);

            return new ServiceResponse<List<Show>>(mappesShowsList);
        }
    }
}
