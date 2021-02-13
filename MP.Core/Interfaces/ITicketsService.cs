using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface ITicketsService
    {
        Task<List<Ticket>> GetAllAsync();
        Task<Ticket> GetAsync(int id);
        Task<Ticket> SaveAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task<Ticket> DeleteAsync(int id);
    }
}
