using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Core.Response;
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

        public async Task<ServiceResponse<List<Room>>> GetAllAsync()
        {
            var rooms = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .ToListAsync();

            var mappedRoomsList = _mapper.Map<List<Room>>(rooms);

            return new ServiceResponse<List<Room>>(mappedRoomsList);
        }

        public async Task<ServiceResponse<Room>> GetAsync(int id)
        {
            var room = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (room == null)
                return new ServiceResponse<Room>(new Error(ErrorCodes.RoomNotExists, ErrorMessages.RoomNotExists(id)));

            var mappedRoom = _mapper.Map<Room>(room);

            return new ServiceResponse<Room>(mappedRoom);
        }

        public async Task<ServiceResponse<Room>> SaveAsync(Room room)
        {
            var mappedRoom = _mapper.Map<DataAccess.EntityModels.Room>(room);

            var cinema = await _dataContext.Cinemas
                .SingleOrDefaultAsync(c => c.Id == room.CinemaId);

            if (cinema == null)
                return new ServiceResponse<Room>(new Error(ErrorCodes.CinemaNotExist, ErrorMessages.CinemaNotExists(room.CinemaId)));

            mappedRoom.Cinema = cinema;

            await _dataContext.Rooms.AddAsync(mappedRoom);
            await _dataContext.SaveChangesAsync();

            var savedRoom = _mapper.Map<Room>(mappedRoom);

            return new ServiceResponse<Room>(savedRoom);
        }

        public async Task<ServiceResponse<Room>> UpdateAsync(Room room)
        {
            var result = await _dataContext.Rooms
                .Include(r => r.Cinema)
                .SingleOrDefaultAsync(r => r.Id == room.Id);

            if (result == null)
                return new ServiceResponse<Room>(new Error(ErrorCodes.RoomNotExists, ErrorMessages.RoomNotExists(room.Id)));

            var cinema = await _dataContext.Cinemas
                .SingleOrDefaultAsync(c => c.Id == room.CinemaId);

            if (cinema == null)
                return new ServiceResponse<Room>(new Error(ErrorCodes.CinemaNotExist, ErrorMessages.CinemaNotExists(room.CinemaId)));

            result.Name = room.Name;
            result.Capacity = room.Capacity;
            result.TicketValue = room.TicketValue;
            result.CinemaId = room.CinemaId;
            result.Cinema = cinema;

            await _dataContext.SaveChangesAsync();

            var updatedRoom = _mapper.Map<Room>(result);

            return new ServiceResponse<Room>(updatedRoom);
        }

        public async Task<ServiceResponse<Room>> DeleteAsync(int id)
        {
            var roomToDelete = await _dataContext.Rooms.SingleOrDefaultAsync(r => r.Id == id);

            if (roomToDelete == null)
                return new ServiceResponse<Room>(new Error(ErrorCodes.RoomNotExists, ErrorMessages.RoomNotExists(id)));

            _dataContext.Remove(roomToDelete);
            await _dataContext.SaveChangesAsync();

            var deletedRoom = _mapper.Map<Room>(roomToDelete);

            return new ServiceResponse<Room>(deletedRoom);
        }
    }
}
