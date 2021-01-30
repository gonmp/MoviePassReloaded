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
    public class CinemasService : ICinemasService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public CinemasService(DataAccessContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<Cinema>> GetAllAsync()
        {
            var cinemas = await _dataContext.Cinemas.ToListAsync();

            return _mapper.Map<List<Cinema>>(cinemas);
        }

        public async Task<Cinema> GetAsync(int id)
        {
            var cinema = await _dataContext.Cinemas.SingleOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<Cinema>(cinema);
        }

        public async Task<Cinema> SaveAsync(Cinema cinema)
        {
            var mappedCinema = _mapper.Map<DataAccess.EntityModels.Cinema>(cinema);

            await _dataContext.Cinemas.AddAsync(mappedCinema);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Cinema>(mappedCinema);
        }

        public async Task<Cinema> UpdateAsync(Cinema cinema)
        {
            var result = await _dataContext.Cinemas.SingleOrDefaultAsync(c => c.Id == cinema.Id);
            if (result == null)
            {
                throw new System.ArgumentException("There is no cinema with Id = " + cinema.Id, "cinema.Id");
            }

            result.Name = cinema.Name;
            result.Address = cinema.Address;

            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Cinema>(result);
        }

        public async Task<Cinema> DeleteAsync(int id)
        {
            var cinemaToDelete = await _dataContext.Cinemas.SingleOrDefaultAsync(c => c.Id == id);

            if (cinemaToDelete == null) return null;

            _dataContext.Remove(cinemaToDelete);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Cinema>(cinemaToDelete);
        }
    }
}
