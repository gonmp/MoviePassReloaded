using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface ICinemasService
    {
        Task<List<Cinema>> GetAllAsync();
        Task<Cinema> GetAsync(int id);
        Task<Cinema> SaveAsync(Cinema cinema);
        Task<Cinema> UpdateAsync(Cinema cinema);
        Task<Cinema> DeleteAsync(int id);
    }
}
