using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public RoomsService(DataAccessContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            var rooms = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .ToListAsync();

            return _mapper.Map<List<Room>>(rooms);
        }

        public async Task<Room> GetAsync(int id)
        {
            var room = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<Room>(room);
        }

        public async Task<Room> SaveAsync(Room room)
        {
            var mappedRoom = _mapper.Map<DataAccess.EntityModels.Room>(room);

            var cinema = await _dataContext.Cinemas
                .SingleOrDefaultAsync(c => c.Id == room.CinemaId);

            if (cinema == null)
            {
                throw new System.ArgumentException("There is no cinema with Id = " + cinema.Id, "cinema.Id");
            }

            mappedRoom.Cinema = cinema;

            await _dataContext.Rooms.AddAsync(mappedRoom);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Room>(mappedRoom);
        }

        public async Task<Room> UpdateAsync(Room room)
        {
            var result = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(r => r.Id == room.Id);

            if (result == null)
            {
                throw new System.ArgumentException("There is no room with Id = " + room.Id, "room.Id");
            }

            var cinema = await _dataContext.Cinemas
                .SingleOrDefaultAsync(c => c.Id == room.CinemaId);

            if (cinema == null)
            {
                throw new System.ArgumentException("There is no cinema with Id = " + cinema.Id, "cinema.Id");
            }

            result.Name = room.Name;
            result.Capacity = room.Capacity;
            result.TicketValue = room.TicketValue;
            result.CinemaId = room.CinemaId;
            result.Cinema = cinema;

            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Room>(result);
        }

        public async Task<Room> DeleteAsync(int id)
        {
            var roomToDelete = await _dataContext.Rooms.SingleOrDefaultAsync(r => r.Id == id);

            if (roomToDelete == null) return null;

            _dataContext.Remove(roomToDelete);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Room>(roomToDelete);
        }
    }
}
