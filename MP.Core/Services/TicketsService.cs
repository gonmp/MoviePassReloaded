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
    public class TicketsService : ITicketsService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public TicketsService(DataAccessContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<Ticket>>> GetAllAsync()
        {
            var tickets = await _dataContext.Tickets
                .Include(t => t.Show)
                    .ThenInclude(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(t => t.Show)
                    .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(t => t.Purchase)
                    .ThenInclude(p => p.User)
                .ToListAsync();

            var mappedTicketsList = _mapper.Map<List<Ticket>>(tickets);

            return new ServiceResponse<List<Ticket>>(mappedTicketsList);
        }

        public async Task<ServiceResponse<Ticket>> GetAsync(int id)
        {
            var ticket = await _dataContext.Tickets
                .Include(t => t.Show)
                    .ThenInclude(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(t => t.Show)
                    .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(t => t.Purchase)
                    .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(t => t.Id == id);

            var mappedTicket = _mapper.Map<Ticket>(ticket);

            return new ServiceResponse<Ticket>(mappedTicket);
        }

        public async Task<ServiceResponse<Ticket>> SaveAsync(Ticket ticket)
        {
            var mappedTicket = _mapper.Map<DataAccess.EntityModels.Ticket>(ticket);

            var show = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(t => t.Id == ticket.ShowId);

            if (show == null)
                return new ServiceResponse<Ticket>(new Error(ErrorCodes.ShowNotExists, ErrorMessages.ShowNotExists(ticket.ShowId)));

            var purchase = await _dataContext.Purchases
                .Include(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == ticket.PurchaseId);

            if (purchase == null)
                return new ServiceResponse<Ticket>(new Error(ErrorCodes.PurchaseNotExists, ErrorMessages.PurchaseNotExists(ticket.PurchaseId)));

            mappedTicket.Purchase = purchase;
            mappedTicket.Show = show;

            await _dataContext.Tickets.AddAsync(mappedTicket);
            await _dataContext.SaveChangesAsync();

            var savedTicket = _mapper.Map<Ticket>(mappedTicket);

            return new ServiceResponse<Ticket>(savedTicket);
        }

        public async Task<ServiceResponse<Ticket>> UpdateAsync(Ticket ticket)
        {
            var result = await _dataContext.Tickets
                .Include(t => t.Show)
                    .ThenInclude(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(t => t.Show)
                    .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(t => t.Purchase)
                    .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(t => t.Id == ticket.Id);

            if (result == null)
                return new ServiceResponse<Ticket>(new Error(ErrorCodes.TicketNotExists, ErrorMessages.TicketNotExists(ticket.Id)));

            var show = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(t => t.Id == ticket.ShowId);

            if (show == null)
                return new ServiceResponse<Ticket>(new Error(ErrorCodes.ShowNotExists, ErrorMessages.ShowNotExists(ticket.ShowId)));

            var purchase = await _dataContext.Purchases
                .Include(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == ticket.PurchaseId);

            if (purchase == null)
                return new ServiceResponse<Ticket>(new Error(ErrorCodes.PurchaseNotExists, ErrorMessages.PurchaseNotExists(ticket.PurchaseId)));

            result.Show = show;
            result.Purchase = purchase;

            await _dataContext.SaveChangesAsync();

            var updatedTicket = _mapper.Map<Ticket>(result);

            return new ServiceResponse<Ticket>(updatedTicket);
        }

        public async Task<ServiceResponse<Ticket>> DeleteAsync(int id)
        {
            var ticketToDelete = await _dataContext.Tickets.SingleOrDefaultAsync(t => t.Id == id);

            if (ticketToDelete == null)
                return new ServiceResponse<Ticket>(new Error(ErrorCodes.TicketNotExists, ErrorMessages.TicketNotExists(id)));

            _dataContext.Remove(ticketToDelete);
            await _dataContext.SaveChangesAsync();

            var deletedTicket = _mapper.Map<Ticket>(ticketToDelete);

            return new ServiceResponse<Ticket>(deletedTicket);
        }
    }
}
