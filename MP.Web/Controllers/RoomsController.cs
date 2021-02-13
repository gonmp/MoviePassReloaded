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
            var rooms = await _roomsService.GetAllAsync();

            var roomsDto = _mapper.Map<List<RoomDTO>>(rooms);

            return Ok(roomsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var room = await _roomsService.GetAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RoomDTO>(room));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(RoomUpsertDTO roomDto)
        {
            var room = await _roomsService.SaveAsync(_mapper.Map<Room>(roomDto));

            return Ok(_mapper.Map<RoomDTO>(room));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(RoomUpsertDTO roomDto, int id)
        {
            var room = await _roomsService.GetAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            _mapper.Map(roomDto, room);

            var updatedCinema = _mapper.Map<RoomDTO>(await _roomsService.UpdateAsync(room));

            return Ok(updatedCinema);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var room = await _roomsService.GetAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            var deletedRoom = _roomsService.DeleteAsync(id);

            return Ok(_mapper.Map<RoomDTO>(deletedRoom));
        }
    }
}
