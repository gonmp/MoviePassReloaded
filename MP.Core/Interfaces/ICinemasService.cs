using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface ICinemasService
    {
        Task<ServiceResponse<List<Cinema>>> GetAllAsync();
        Task<ServiceResponse<Cinema>> GetAsync(int id);
        Task<ServiceResponse<Cinema>> SaveAsync(Cinema cinema);
        Task<ServiceResponse<Cinema>> UpdateAsync(Cinema cinema);
        Task<ServiceResponse<Cinema>> DeleteAsync(int id);
    }
}
