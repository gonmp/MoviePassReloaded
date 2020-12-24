using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.DataAccess;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public class MoviesService : IMoviesService
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public MoviesService(DataAccessContext dataContext, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<Movie> DeleteAsync(int id)
        {
            var movieToDelete = await _dataContext.Movies.SingleAsync(m => m.Id == id);
            _dataContext.Movies.Remove(movieToDelete);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Movie>(movieToDelete);
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            var moviesList = await _dataContext.Movies.ToListAsync();

            return _mapper.Map<List<Movie>>(moviesList);
        }

        public async Task<List<Movie>> GetAllFromApiAsync()
        {
            var totalPages = await GetTotalPages();
            var httpClient = _httpClientFactory.CreateClient();
            var movies = new List<Movie>();
            for (int i = 1; i <= totalPages; i++)
            {
                var response = await httpClient.GetAsync($"https://api.themoviedb.org/3/movie/now_playing?api_key=fdd94e2a3c2efe450f97426c853e8fcf&language=es-AR&page={i}");
                var result = await response.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(result);
                var moviesJson = resultJson.SelectToken("results");                
                foreach (var movieJson in moviesJson)
                {
                    var id = (int)movieJson.SelectToken("id");
                    var response2 = await httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{id}?api_key=fdd94e2a3c2efe450f97426c853e8fcf&language=en-US");
                    var result2 = await response2.Content.ReadAsStringAsync();
                    var resultJson2 = JObject.Parse(result2);
                    var duration = (int)resultJson2.SelectToken("runtime");
                    var movie = new Movie
                    {
                        Id = id,
                        Title = (string)movieJson.SelectToken("title"),
                        Language = (string)movieJson.SelectToken("original_language"),
                        Image = (string)movieJson.SelectToken("poster_path"),
                        Overview = (string)movieJson.SelectToken("overview"),
                        Duration = duration
                    };
                    await _dataContext.Movies.AddAsync(_mapper.Map<DataAccess.EntityModels.Movie>(movie));
                    movies.Add(movie);
                }
            }
            await _dataContext.SaveChangesAsync();
            return movies;
        }

        private async Task<int> GetTotalPages()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://api.themoviedb.org/3/movie/now_playing?api_key=fdd94e2a3c2efe450f97426c853e8fcf&language=en-US&page=1");
            var result = await response.Content.ReadAsStringAsync();
            var resultJson = JObject.Parse(result);
            var totalPages = (int)resultJson.SelectToken("total_pages");

            return totalPages;
        }

        public async Task<Movie> GetAsync(int id)
        {
            var movie = await _dataContext.Movies.SingleOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<Movie>(movie);
        }

        public async Task<Movie> SaveAsync(Movie movie)
        {
            var result = _dataContext.Movies.SingleOrDefaultAsync(m => m.Id == movie.Id);

            if(result != null)
            {
                throw new System.ArgumentException("There is already a movie with Id = " + movie.Id, "movie.Id");
            }

            await _dataContext.Movies.AddAsync(_mapper.Map<DataAccess.EntityModels.Movie>(movie));

            return movie;
        }

        public async Task<Movie> UpdateAsync(Movie movie)
        {
            var result = await _dataContext.Movies.SingleOrDefaultAsync(m => m.Id == movie.Id);
            if (result == null)
            {
                throw new System.ArgumentException("There is no movie with Id = " + movie.Id, "movie.Id");
            }

            result.Id = movie.Id;
            result.Duration = movie.Duration;
            result.Image = movie.Image;
            result.Language = movie.Language;
            result.Overview = movie.Overview;
            result.Title = movie.Title;

            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Movie>(result);
        }
    }
}
