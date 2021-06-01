using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IGenresService
    {
        Task<ServiceResponse<List<Genre>>> GetAllFromApiAsync();
        Task<ServiceResponse<List<Genre>>> GetAllAsync();
        Task<ServiceResponse<Genre>> GetAsync(int id);
        Task<ServiceResponse<Genre>> SaveAsync(Genre genre);
        Task<ServiceResponse<Genre>> UpdateAsync(Genre genre);
        Task<ServiceResponse<Genre>> DeleteAsync(int id);
    }
}
