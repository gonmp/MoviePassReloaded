using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IProfilesService
    {
        Task<ServiceResponse<List<Profile>>> GetAllAsync();
        Task<ServiceResponse<Profile>> GetAsync(int userId);
        Task<ServiceResponse<Profile>> SaveAsync(int userId, Profile profile);
        Task<ServiceResponse<Profile>> UpdateAsync(Profile profile);
        Task<ServiceResponse<Profile>> DeleteAsync(int id);
    }
}
