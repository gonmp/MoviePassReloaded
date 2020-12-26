using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IGenresService
    {
        Task<List<Genre>> GetAllFromApiAsync();
        Task<List<Genre>> GetAllAsync();
        Task<Genre> GetAsync(int id);
        Task<Genre> SaveAsync(Genre movie);
        Task<Genre> UpdateAsync(Genre movie);
        Task<Genre> DeleteAsync(int id);
    }
}
