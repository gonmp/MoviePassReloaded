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
    public class TicketsService : ITicketsService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public TicketsService(DataAccessContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<Ticket>> GetAllAsync()
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

            return _mapper.Map<List<Ticket>>(tickets);
        }

        public async Task<Ticket> GetAsync(int id)
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

            return _mapper.Map<Ticket>(ticket);
        }

        public async Task<Ticket> SaveAsync(Ticket ticket)
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
            {
                throw new System.ArgumentException("There is no show with Id = " + show.Id, "show.Id");
            }

            var purchase = await _dataContext.Purchases
                .Include(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == ticket.PurchaseId);
            if (purchase == null)
            {
                throw new System.ArgumentException("There is no purchase with Id = " + purchase.Id, "purchase.Id");
            }

            mappedTicket.Purchase = purchase;
            mappedTicket.Show = show;

            await _dataContext.Tickets.AddAsync(mappedTicket);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Ticket>(mappedTicket);
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
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
            {
                throw new System.ArgumentException("There is no ticket with Id = " + ticket.Id, "ticket.Id");
            }

            var show = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(t => t.Id == ticket.ShowId);
            if (show == null)
            {
                throw new System.ArgumentException("There is no show with Id = " + show.Id, "show.Id");
            }

            var purchase = await _dataContext.Purchases
                .Include(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == ticket.PurchaseId);
            if (purchase == null)
            {
                throw new System.ArgumentException("There is no purchase with Id = " + purchase.Id, "purchase.Id");
            }

            result.Show = show;
            result.Purchase = purchase;

            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Ticket>(result);
        }

        public async Task<Ticket> DeleteAsync(int id)
        {
            var ticketToDelete = await _dataContext.Tickets.SingleOrDefaultAsync(t => t.Id == id);

            if (ticketToDelete == null) return null;

            _dataContext.Remove(ticketToDelete);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Ticket>(ticketToDelete);
        }
    }
}
