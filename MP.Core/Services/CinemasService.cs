using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Core.Response;
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

        public async Task<ServiceResponse<List<Cinema>>> GetAllAsync()
        {
            var cinemas = await _dataContext.Cinemas.ToListAsync();

            var mappedCinemasList = _mapper.Map<List<Cinema>>(cinemas);

            return new ServiceResponse<List<Cinema>>(mappedCinemasList);
        }

        public async Task<ServiceResponse<Cinema>> GetAsync(int id)
        {
            var cinema = await _dataContext.Cinemas.SingleOrDefaultAsync(c => c.Id == id);

            if (cinema == null)
                return new ServiceResponse<Cinema>(new Error(ErrorCodes.CinemaNotExist, ErrorMessages.CinemaNotExists(id)));

            var mappedCinema = _mapper.Map<Cinema>(cinema);

            return new ServiceResponse<Cinema>(mappedCinema);
        }

        public async Task<ServiceResponse<Cinema>> SaveAsync(Cinema cinema)
        {
            var mappedCinema = _mapper.Map<DataAccess.EntityModels.Cinema>(cinema);

            await _dataContext.Cinemas.AddAsync(mappedCinema);
            await _dataContext.SaveChangesAsync();

            var savedCiname = _mapper.Map<Cinema>(mappedCinema);

            return new ServiceResponse<Cinema>(savedCiname);
        }

        public async Task<ServiceResponse<Cinema>> UpdateAsync(Cinema cinema)
        {
            var result = await _dataContext.Cinemas.SingleOrDefaultAsync(c => c.Id == cinema.Id);
            if (result == null)
                return new ServiceResponse<Cinema>(new Error(ErrorCodes.CinemaNotExist, ErrorMessages.CinemaNotExists(cinema.Id)));

            result.Name = cinema.Name;
            result.Address = cinema.Address;

            await _dataContext.SaveChangesAsync();

            var updatedCinema = _mapper.Map<Cinema>(result);

            return new ServiceResponse<Cinema>(updatedCinema);
        }

        public async Task<ServiceResponse<Cinema>> DeleteAsync(int id)
        {
            var cinemaToDelete = await _dataContext.Cinemas.SingleOrDefaultAsync(c => c.Id == id);

            if (cinemaToDelete == null)
                return new ServiceResponse<Cinema>(new Error(ErrorCodes.CinemaNotExist, ErrorMessages.CinemaNotExists(id)));

            _dataContext.Remove(cinemaToDelete);
            await _dataContext.SaveChangesAsync();

            var deletedCinema = _mapper.Map<Cinema>(cinemaToDelete);

            return new ServiceResponse<Cinema>(deletedCinema);
        }
    }
}
