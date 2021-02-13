using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.DataAccess;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public class PurchasesService : IPurchasesService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ITicketsService _ticketsService;

        public PurchasesService(DataAccessContext dataContext, IMapper mapper, ITicketsService ticketsService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _ticketsService = ticketsService;
        }

        public async Task<List<Purchase>> GetAllAsync()
        {
            var purchases = await _dataContext.Purchases
                .Include(p => p.User)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .ToListAsync();

            return _mapper.Map<List<Purchase>>(purchases);
        }

        public async Task<List<Purchase>> GetAllAsync(int userId)
        {
            var purchases = await _dataContext.Purchases
                .Include(p => p.User)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<Purchase>>(purchases);
        }

        public async Task<Purchase> GetAsync(int id)
        {
            var purchase = await _dataContext.Purchases
                .Include(p => p.User)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .SingleOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<Purchase>(purchase);
        }

        public async Task<Purchase> SaveAsync(int numberOfTickets, int userId, int showId, string cardNumber, int expMonth, int expYear, string cvc)
        {

            var user = await _dataContext.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new System.ArgumentException("There is no user with Id = " + user.Id, "user.Id");
            }

            var show = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .SingleOrDefaultAsync(s => s.Id == showId);

            if (show == null)
            {
                throw new System.ArgumentException("There is no show with Id = " + show.Id, "show.Id");
            }

            if (!ValidatePayment(cardNumber, expMonth, expYear, cvc))
            {
                throw new System.ArgumentException("Invalid payment");
            }
            
            var mappedPurchase = new DataAccess.EntityModels.Purchase();
            mappedPurchase.NumberOfTickets = numberOfTickets;
            mappedPurchase.PurchaseDate = DateTime.Now;
            mappedPurchase.TotalNoDiscount = show.Room.TicketValue * mappedPurchase.NumberOfTickets;
            mappedPurchase.Discount = 0;
            mappedPurchase.TotalWithDiscount = mappedPurchase.TotalNoDiscount * (1 - mappedPurchase.Discount);
            mappedPurchase.UserId = userId;
            mappedPurchase.User = user;

            await _dataContext.Purchases.AddAsync(mappedPurchase);

            var tickets = new List<DataAccess.EntityModels.Ticket>();

            for(var i = 0; i < numberOfTickets; i++)
            {
                var newTicket = new DataAccess.EntityModels.Ticket();
                newTicket.Code = Guid.NewGuid();
                newTicket.PurchaseId = mappedPurchase.Id;
                newTicket.Purchase = mappedPurchase;
                newTicket.ShowId = show.Id;
                newTicket.Show = _mapper.Map<DataAccess.EntityModels.Show>(show);
                newTicket.Qr = $"https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl={newTicket.Code}/{newTicket.Show.Movie.Title}/{newTicket.Show.Room.Cinema.Name}/{newTicket.Show.Room.Name}/{newTicket.Show.DateTime.ToString()}";

                await _dataContext.Tickets.AddAsync(newTicket);

                tickets.Add(newTicket);
            }

            mappedPurchase.Tickets = tickets;
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Purchase>(mappedPurchase);
        }

        public async Task<Purchase> UpdateAsync(Purchase purchase)
        {
            var result = await _dataContext.Purchases
                .Include(p => p.User)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Show)
                    .ThenInclude(s => s.Movie)
                    .ThenInclude(m => m.MoviesGenres)
                    .ThenInclude(mg => mg.Genre)
                .SingleOrDefaultAsync(p => p.Id == purchase.Id);

            if (result == null)
            {
                throw new System.ArgumentException("There is no purchase with Id = " + purchase.Id, "purchase.Id");
            }

            var user = await _dataContext.Users
                .SingleOrDefaultAsync(u => u.Id == purchase.Id);
            if (user == null)
            {
                throw new System.ArgumentException("There is no user with Id = " + user.Id, "user.Id");
            }

            var tickets = new List<DataAccess.EntityModels.Ticket>();

            foreach (var ticketId in purchase.Tickets)
            {
                var ticket = await _dataContext.Tickets
                    .Include(t => t.Show)
                        .ThenInclude(s => s.Room)
                        .ThenInclude(r => r.Cinema)
                    .Include(t => t.Show)
                        .ThenInclude(s => s.Movie)
                        .ThenInclude(m => m.MoviesGenres)
                        .ThenInclude(mg => mg.Genre)
                    .SingleOrDefaultAsync(t => t.Id == ticketId.Id);

                if (ticket == null)
                {
                    throw new System.ArgumentException("There is no ticket with Id = " + ticketId.Id, "ticket.Id");
                }

                tickets.Add(ticket);
            }

            result.User = user;
            result.Tickets = tickets;

            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Purchase>(result);
        }

        public async Task<Purchase> DeleteAsync(int id)
        {
            var purchaseToDelete = await _dataContext.Purchases.SingleOrDefaultAsync(p => p.Id == id);

            if (purchaseToDelete == null) return null;

            _dataContext.Remove(purchaseToDelete);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Purchase>(purchaseToDelete);
        }

        private bool ValidatePayment(string cardNumber, int expMonth, int expYear, string cvc)
        {
            StripeConfiguration.ApiKey = "sk_test_51HqJvNKO2Y0Jg2sUua2odEVQdzvU0ii3xCg5aC9o92AECMfEsE4518uDUZCxwod1fcypzeuSWBlYaUmBYs2k8YZy00rLKQGklE";
            var flag = false;
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Number = cardNumber,
                    ExpMonth = expMonth,
                    ExpYear = expYear,
                    Cvc = cvc,
                },
            };
            var service = new PaymentMethodService();
            var response = service.Create(options);

            if(response.Card.Checks.CvcCheck.Equals("pass") || response.Card.Checks.CvcCheck.Equals("unchecked"))
            {
                flag = true;
            }

            return flag;
        }
    }
}
