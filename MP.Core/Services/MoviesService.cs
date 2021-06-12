using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MP.Core.Configuration;
using MP.Core.Helpers;
using MP.Core.Models;
using MP.Core.Resources;
using MP.Core.Response;
using MP.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;
        private readonly TheMovieDbOptions _theMovieDbOptions;

        public MoviesService(DataAccessContext dataContext, IMapper mapper, IHttpClientFactory httpClientFactory, IOptions<TheMovieDbOptions> theMovieDbOptions)
        {
            _httpClientFactory = httpClientFactory;
            _dataContext = dataContext;
            _mapper = mapper;
            _theMovieDbOptions = theMovieDbOptions.Value;
        }

        public async Task<ServiceResponse<Movie>> DeleteAsync(int id)
        {
            var movieToDelete = await _dataContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movieToDelete == null)
                return new ServiceResponse<Movie>(new Error(ErrorCodes.MovieNotExists, ErrorMessages.MovieNotExists(id)));

            _dataContext.Movies.Remove(movieToDelete);

            await _dataContext.SaveChangesAsync();

            return new ServiceResponse<Movie>(_mapper.Map<Movie>(movieToDelete));
        }

        public async Task<ServiceResponse<PagedList<Movie>>> GetAsync(string movieTitle, int pageSize, int pageNumber)
        {
            var moviesList = await _dataContext.Movies.Include(m => m.Genres).ToListAsync();

            if (!string.IsNullOrEmpty(movieTitle))
                moviesList = moviesList.Where(m => m.Title.Contains(movieTitle)).ToList();

            var mappedMovies = _mapper.Map<List<Movie>>(moviesList);

            var pagedListMovies = PagedList<Movie>.ToPagedList(mappedMovies, pageNumber, pageSize);

            return new ServiceResponse<PagedList<Movie>>(pagedListMovies);
        }

        public async Task<ServiceResponse<Movie>> GetAsync(int id)
        {
            var movie = await _dataContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return new ServiceResponse<Movie>(new Error(ErrorCodes.MovieNotExists, ErrorMessages.MovieNotExists(id)));

            var mappedMovie = _mapper.Map<Movie>(movie);

            return new ServiceResponse<Movie>(mappedMovie);
        }

        public async Task<List<Movie>> GetAllFromApiAsync()
        {
            var totalPages = await GetTotalPages();
            var httpClient = _httpClientFactory.CreateClient();
            var movies = new List<Movie>();
            for (int i = 1; i <= totalPages; i++)
            {
                var response = await httpClient.GetAsync(TheMovieDbUris.GetMoviesNowPlaying(i, _theMovieDbOptions.ApiKey));
                var result = await response.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(result);
                var moviesJson = resultJson.SelectToken("results");                
                foreach (var movieJson in moviesJson)
                {
                    var id = (int)movieJson.SelectToken("id");
                    var response2 = await httpClient.GetAsync(TheMovieDbUris.GetMovie(id, _theMovieDbOptions.ApiKey));
                    var result2 = await response2.Content.ReadAsStringAsync();
                    var resultJson2 = JObject.Parse(result2);
                    var durationJson = resultJson2.SelectToken("runtime");
                    var duration = 0;
                    if(durationJson.Type != JTokenType.Null)
                    {
                        duration = (int)durationJson;
                    }
                    var genresIdsJson = movieJson.SelectToken("genre_ids");
                    var genresList = new List<DataAccess.EntityModels.Genre>();
                    foreach (var genreIdJson in genresIdsJson)
                    {
                        var genreId = (int)genreIdJson;
                        var genre = await _dataContext.Genres.Include(m => m.Movies).SingleOrDefaultAsync(g => g.Id == genreId);
                        genresList.Add(genre);
                    }
                    var movie = new Movie
                    {
                        Id = id,
                        Title = (string)movieJson.SelectToken("title"),
                        Language = (string)movieJson.SelectToken("original_language"),
                        Image = (string)movieJson.SelectToken("poster_path"),
                        Overview = (string)movieJson.SelectToken("overview"),
                        Duration = duration
                    };

                    var mappedMovie = _mapper.Map<DataAccess.EntityModels.Movie>(movie);
                    await _dataContext.Movies.AddAsync(mappedMovie);

                    foreach(var genre in genresList)
                    {
                        mappedMovie.Genres.Add(genre);
                    }

                    
                    movies.Add(_mapper.Map<Movie>(mappedMovie));

                }
            }
            await _dataContext.SaveChangesAsync();
            return movies;
        }

        private async Task<int> GetTotalPages()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(TheMovieDbUris.GetMoviesNowPlaying(1, _theMovieDbOptions.ApiKey));
            var result = await response.Content.ReadAsStringAsync();
            var resultJson = JObject.Parse(result);
            var totalPages = (int)resultJson.SelectToken("total_pages");

            return totalPages;
        }        

        public async Task<ServiceResponse<Movie>> SaveAsync(Movie movie)
        {
            var genresList = new List<DataAccess.EntityModels.Genre>();
            foreach(var genre in movie.Genres)
            {
                var genreToAdd = await _dataContext.Genres.SingleOrDefaultAsync(g => g.Id == genre.Id);

                if (genreToAdd == null)
                    return new ServiceResponse<Movie>(new Error(ErrorCodes.GenreNotExists, ErrorMessages.GenreNotExists(genre.Id)));

                genresList.Add(genreToAdd);
            }

            var mappedMovie = _mapper.Map<DataAccess.EntityModels.Movie>(movie);

            mappedMovie.Genres = genresList;

            await _dataContext.Movies.AddAsync(mappedMovie);

            await _dataContext.SaveChangesAsync();

            var savedMovie = _mapper.Map<Movie>(mappedMovie);

            return new ServiceResponse<Movie>(savedMovie);
        }

        public async Task<ServiceResponse<Movie>> UpdateAsync(Movie movie)
        {
            var result = await _dataContext.Movies.Include(m => m.Genres).SingleOrDefaultAsync(m => m.Id == movie.Id);

            if (result == null)
                return new ServiceResponse<Movie>(new Error(ErrorCodes.MovieNotExists, ErrorMessages.MovieNotExists(movie.Id)));

            var genresList = new List<DataAccess.EntityModels.Genre>();

            foreach (var genre in movie.Genres)
            {
                var genreToAdd = await _dataContext.Genres.SingleOrDefaultAsync(g => g.Id == genre.Id);

                if (genreToAdd == null)
                    return new ServiceResponse<Movie>(new Error(ErrorCodes.GenreNotExists, ErrorMessages.GenreNotExists(genre.Id)));

                genresList.Add(genreToAdd);
            }

            result.Id = movie.Id;
            result.Duration = movie.Duration;
            result.Image = movie.Image;
            result.Language = movie.Language;
            result.Overview = movie.Overview;
            result.Title = movie.Title;
            result.Genres = genresList;

            await _dataContext.SaveChangesAsync();

            var updatedMovie = _mapper.Map<Movie>(result);

            return new ServiceResponse<Movie>(updatedMovie);
        }

        public async Task<ServiceResponse<List<Movie>>> GetMovieListingsAsync()
        {
            var nextThursday = Dates.GetNextDayOfWeek((int)DayOfWeek.Thursday);
            var today = DateTime.Now;

            var movies = await _dataContext.Shows
                .Include(s => s.Movie)
                    .ThenInclude(m => m.Genres)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Cinema)
                .Where(s => s.DateTime >= today && s.DateTime <= nextThursday)
                .Select(s => s.Movie)
                .ToListAsync();

            var mappedMoviesList = _mapper.Map<List<Movie>>(movies);

            return new ServiceResponse<List<Movie>>(mappedMoviesList);
        }
    }
}
