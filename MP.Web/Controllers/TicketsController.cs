using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Web.Dtos.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITicketsService _ticketsService;

        public TicketsController(IMapper mapper, ITicketsService ticketsService)
        {
            _mapper = mapper;
            _ticketsService = ticketsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var tickets = await _ticketsService.GetAllAsync();

            var ticketsDto = _mapper.Map<List<TicketDTO>>(tickets);

            return Ok(ticketsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var ticket = await _ticketsService.GetAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TicketDTO>(ticket));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(TicketUpsertDTO ticketDto)
        {
            var ticket = await _ticketsService.SaveAsync(_mapper.Map<Ticket>(ticketDto));

            return Ok(_mapper.Map<TicketDTO>(ticket));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(TicketUpsertDTO ticketDto, int id)
        {
            var ticket = await _ticketsService.GetAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _mapper.Map(ticketDto, ticket);

            var updatedTicket = _mapper.Map<TicketDTO>(await _ticketsService.UpdateAsync(ticket));

            return Ok(updatedTicket);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var ticket = await _ticketsService.GetAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var deletedTicket = _ticketsService.DeleteAsync(id);

            return Ok(_mapper.Map<TicketDTO>(deletedTicket));
        }
    }
}
