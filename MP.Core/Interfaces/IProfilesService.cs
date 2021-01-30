using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IProfilesService
    {
        Task<Profile> DeleteAsync(int id);
        Task<List<Profile>> GetAllAsync();
        Task<Profile> GetAsync(int userId);
        Task<Profile> SaveAsync(int userId, Profile profile);
        Task<Profile> UpdateAsync(Profile profile);
    }
}
