using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface ITicketsService
    {
        Task<ServiceResponse<List<Ticket>>> GetAllAsync();
        Task<ServiceResponse<Ticket>> GetAsync(int id);
        Task<ServiceResponse<Ticket>> SaveAsync(Ticket ticket);
        Task<ServiceResponse<Ticket>> UpdateAsync(Ticket ticket);
        Task<ServiceResponse<Ticket>> DeleteAsync(int id);
    }
}
