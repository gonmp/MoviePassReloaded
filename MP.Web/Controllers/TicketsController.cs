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
            var ticketsResponse = await _ticketsService.GetAllAsync();

            var tickets = ticketsResponse.Content;

            var ticketsDto = _mapper.Map<List<TicketDto>>(tickets);

            return Ok(ticketsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var ticketResponse = await _ticketsService.GetAsync(id);

            if (!ticketResponse.Success)
                return NotFound(ticketResponse.Error);

            var ticket = ticketResponse.Content;

            var ticketDto = _mapper.Map<TicketDto>(ticket);

            return Ok(ticketDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(TicketUpsertDto ticketDto)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);

            var ticketResponse = await _ticketsService.SaveAsync(ticket);

            if (!ticketResponse.Success)
                return NotFound(ticketResponse.Error);

            var savedTicket = ticketResponse.Content;

            var savedTicketDto = _mapper.Map<TicketDto>(savedTicket);

            return Ok(savedTicketDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(TicketUpsertDto ticketDto, int id)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);
            ticket.Id = id;

            var ticketResponse = await _ticketsService.UpdateAsync(ticket);

            if (!ticketResponse.Success)
                return NotFound(ticketResponse.Error);

            var updatedTicket = ticketResponse.Content;

            var updatedTicketDto = _mapper.Map<TicketDto>(updatedTicket);

            return Ok(updatedTicketDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var ticketResponse = await _ticketsService.DeleteAsync(id);

            if (!ticketResponse.Success)
                return NotFound(ticketResponse.Error);

            var deletedTicket = ticketResponse.Content;

            var deletedTicketDto = _mapper.Map<TicketDto>(deletedTicket);

            return Ok(deletedTicketDto);
        }
    }
}
