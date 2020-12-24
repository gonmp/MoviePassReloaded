using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IMoviesService
    {
        Task<List<Movie>> GetAllFromApiAsync();
        Task<List<Movie>> GetAllAsync();
        Task<Movie> GetAsync(int id);
        Task<Movie> SaveAsync(Movie movie);
        Task<Movie> UpdateAsync(Movie movie);
        Task<Movie> DeleteAsync(int id);
    }
}
