using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface IRoomsService
    {
        Task<ServiceResponse<List<Room>>> GetAllAsync();
        Task<ServiceResponse<Room>> GetAsync(int id);
        Task<ServiceResponse<Room>> SaveAsync(Room room);
        Task<ServiceResponse<Room>> UpdateAsync(Room room);
        Task<ServiceResponse<Room>> DeleteAsync(int id);
    }
}
