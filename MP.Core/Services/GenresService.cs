using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Models;
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
        private IHttpClientFactory _httpClientFactory;
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public GenresService(DataAccessContext dataContext, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public Task<Genre> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Genre>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Genre>> GetAllFromApiAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var genres = new List<Genre>();
            
            var response = await httpClient.GetAsync("https://api.themoviedb.org/3/genre/movie/list?api_key=fdd94e2a3c2efe450f97426c853e8fcf&language=es-ES");
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
            return genres;
        }

        public async Task<Genre> GetAsync(int id)
        {
            var genre = await _dataContext.Genres.AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);

            return _mapper.Map<Genre>(genre);
        }

        public Task<Genre> SaveAsync(Genre movie)
        {
            throw new NotImplementedException();
        }

        public Task<Genre> UpdateAsync(Genre movie)
        {
            throw new NotImplementedException();
        }
    }
}
