using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Interfaces
{
    public interface IRoomsService
    {
        Task<List<Room>> GetAllAsync();
        Task<Room> GetAsync(int id);
        Task<Room> SaveAsync(Room room);
        Task<Room> UpdateAsync(Room room);
        Task<Room> DeleteAsync(int id);
    }
}
