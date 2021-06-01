using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface IShowsService
    {
        Task<ServiceResponse<List<Show>>> GetAllAsync();
        Task<ServiceResponse<Show>> GetAsync(int id);
        Task<ServiceResponse<Show>> SaveAsync(Show show);
        Task<ServiceResponse<Show>> UpdateAsync(Show show);
        Task<ServiceResponse<Show>> DeleteAsync(int id);
        Task<ServiceResponse<List<Show>>> GetShowsListingsAsync();
    }
}
