using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MP.Core.Configuration;
using MP.Core.Models;
using MP.Core.Resources;
using MP.Core.Response;
using MP.DataAccess;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public class GenresService : IGenresService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;
        private readonly TheMovieDbOptions _theMovieDbOptions;

        public GenresService(DataAccessContext dataContext, IMapper mapper, IHttpClientFactory httpClientFactory, IOptions<TheMovieDbOptions> theMovieDbOptions)
        {
            _httpClientFactory = httpClientFactory;
            _dataContext = dataContext;
            _mapper = mapper;
            _theMovieDbOptions = theMovieDbOptions.Value;
        }

        public async Task<ServiceResponse<Genre>> DeleteAsync(int id)
        {
            var genreToDelete = await _dataContext.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if (genreToDelete == null)
                return new ServiceResponse<Genre>(new Error(ErrorCodes.GenreNotExists, ErrorMessages.GenreNotExists(id)));

            _dataContext.Remove(genreToDelete);
            await _dataContext.SaveChangesAsync();

            var mappedGenre = _mapper.Map<Genre>(genreToDelete);

            return new ServiceResponse<Genre>(mappedGenre);
        }

        public async Task<ServiceResponse<List<Genre>>> GetAllAsync()
        {
            var genreList = await _dataContext.Genres.ToListAsync();

            var mappedGenreList = _mapper.Map<List<Genre>>(genreList);

            return new ServiceResponse<List<Genre>>(mappedGenreList);
        }

        public async Task<ServiceResponse<List<Genre>>> GetAllFromApiAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var genres = new List<Genre>();
            
            var response = await httpClient.GetAsync(TheMovieDbUris.GetGenres(_theMovieDbOptions.ApiKey));
            var result = await response.Content.ReadAsStringAsync();
            var resultJson = JObject.Parse(result);
            var genresJson = resultJson.SelectToken("genres");
            foreach (var genreJson in genresJson)
            {
                var id = (int)genreJson.SelectToken("id");
                var name = (string)genreJson.SelectToken("name");
                var resultDatabase = await _dataContext.Genres.SingleOrDefaultAsync(g => g.Id == id);
                if (resultDatabase != null)
                {
                    resultDatabase.Name = name;
                }
                else
                {
                    var genre = new Genre
                    {
                        Id = id,
                        Name = name
                    };
                    await _dataContext.Genres.AddAsync(_mapper.Map<DataAccess.EntityModels.Genre>(genre));
                    genres.Add(genre);
                }

            }
            await _dataContext.SaveChangesAsync();
            return new ServiceResponse<List<Genre>>(genres);
        }

        public async Task<ServiceResponse<Genre>> GetAsync(int id)
        {
            var genre = await _dataContext.Genres.AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);

            var mappedGenre = _mapper.Map<Genre>(genre);

            return new ServiceResponse<Genre>(mappedGenre);
        }

        public async Task<ServiceResponse<Genre>> SaveAsync(Genre genre)
        {
            var mappedGenre = _mapper.Map<DataAccess.EntityModels.Genre>(genre);

            await _dataContext.Genres.AddAsync(mappedGenre);
            await _dataContext.SaveChangesAsync();

            var savedGenre = _mapper.Map<Genre>(mappedGenre);

            return new ServiceResponse<Genre>(savedGenre);
        }

        public async Task<ServiceResponse<Genre>> UpdateAsync(Genre genre)
        {
            var result = await _dataContext.Genres.SingleOrDefaultAsync(g => g.Id == genre.Id);
            if (result == null)
                return new ServiceResponse<Genre>(new Error(ErrorCodes.GenreNotExists, ErrorMessages.GenreNotExists(genre.Id)));

            result.Id = genre.Id;
            result.Name = genre.Name;

            await _dataContext.SaveChangesAsync();

            var updatedGenre = _mapper.Map<Genre>(result);

            return new ServiceResponse<Genre>(updatedGenre);
        }
    }
}
