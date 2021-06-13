using MP.Core.Helpers;
using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IMoviesService
    {
        Task<List<Movie>> GetAllFromApiAsync();
        Task<ServiceResponse<PagedList<Movie>>> GetAsync(string movieTitle, int pageSize, int pageNumber);
        Task<ServiceResponse<Movie>> GetAsync(int id);
        Task<ServiceResponse<Movie>> SaveAsync(Movie movie);
        Task<ServiceResponse<Movie>> UpdateAsync(Movie movie);
        Task<ServiceResponse<Movie>> DeleteAsync(int id);
        Task<ServiceResponse<List<Movie>>> GetMovieListingsAsync();
        Task<ServiceResponse<List<Cinema>>> GetCinemasPlayingMovie(int movieId);
    }
}
