using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface IPurchasesService
    {
        Task<ServiceResponse<List<Purchase>>> GetAllAsync();
        Task<ServiceResponse<List<Purchase>>> GetAllAsync(int userId);
        Task<ServiceResponse<Purchase>> GetAsync(int id);
        Task<ServiceResponse<Purchase>> SaveAsync(int numberOfTickets, int userId, int showId, string cardNumber, int expMonth, int expYear, string cvc);
        Task<ServiceResponse<Purchase>> UpdateAsync(Purchase purchase);
        Task<ServiceResponse<Purchase>> DeleteAsync(int id);
    }
}
