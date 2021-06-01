using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Web.Dtos.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomsService _roomsService;

        public RoomsController(IMapper mapper, IRoomsService roomsService)
        {
            _mapper = mapper;
            _roomsService = roomsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var roomsResponse = await _roomsService.GetAllAsync();

            var rooms = roomsResponse.Content;

            var roomsDto = _mapper.Map<List<RoomDto>>(rooms);

            return Ok(roomsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var roomResponse = await _roomsService.GetAsync(id);

            if (!roomResponse.Success)
                return NotFound(roomResponse.Error);

            var room = roomResponse.Content;

            var roomDto = _mapper.Map<RoomDto>(room);

            return Ok(roomDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(RoomUpsertDto roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);

            var roomResponse = await _roomsService.SaveAsync(room);

            if (!roomResponse.Success)
                return NotFound(roomResponse.Error);

            var savedRoom = roomResponse.Content;

            var savedRoomDto = _mapper.Map<RoomDto>(savedRoom);

            return Ok(savedRoomDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(RoomUpsertDto roomDto, int id)
        {
            var room = _mapper.Map<Room>(roomDto);
            room.Id = id;

            var roomResponse = await _roomsService.UpdateAsync(room);

            if (!roomResponse.Success)
                return NotFound(roomResponse.Error);

            var updatedRoom = roomResponse.Content;

            var updatedRoomDto = _mapper.Map<RoomDto>(updatedRoom);

            return Ok(updatedRoomDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var roomResponse = await _roomsService.DeleteAsync(id);

            if (!roomResponse.Success)
                return NotFound(roomResponse.Error);

            var deletedRoom = roomResponse.Content;

            var deletedRoomDto = _mapper.Map<RoomDto>(deletedRoom);

            return Ok(deletedRoomDto);
        }
    }
}
