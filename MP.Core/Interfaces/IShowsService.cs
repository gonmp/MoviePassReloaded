using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface IShowsService
    {
        Task<List<Show>> GetAllAsync();
        Task<Show> GetAsync(int id);
        Task<Show> SaveAsync(Show show);
        Task<Show> UpdateAsync(Show show);
        Task<Room> DeleteAsync(int id);
    }
}
