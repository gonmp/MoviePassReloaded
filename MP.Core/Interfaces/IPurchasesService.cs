using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface IPurchasesService
    {
        Task<List<Purchase>> GetAllAsync();
        Task<List<Purchase>> GetAllAsync(int userId);
        Task<Purchase> GetAsync(int id);
        Task<Purchase> SaveAsync(int numberOfTickets, int userId, int showId, string cardNumber, int expMonth, int expYear, string cvc);
        Task<Purchase> UpdateAsync(Purchase purchase);
        Task<Purchase> DeleteAsync(int id);
    }
}
