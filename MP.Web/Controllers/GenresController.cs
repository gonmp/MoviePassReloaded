using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Models;
using MP.Core.Services;
using MP.Web.Dtos.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenresService _genresService;

        public GenresController(IMapper mapper, IGenresService genresService)
        {
            _mapper = mapper;
            _genresService = genresService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("~/api/themoviesdb/genres")]
        public async Task<IActionResult> GetAllFromApiAsync()
        {
            var genresResponse = await _genresService.GetAllFromApiAsync();

            var genres = genresResponse.Content;

            var genresDto = _mapper.Map<List<GenreDto>>(genres);

            return Ok(genresDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAync()
        {
            var genresResponse = await _genresService.GetAllAsync();

            var genres = genresResponse.Content;

            var genresDto = _mapper.Map<List<GenreDto>>(genres);

            return Ok(genresDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var genreResponse = await _genresService.GetAsync(id);

            if (!genreResponse.Success)
                return NotFound(genreResponse.Error);

            var genre = genreResponse.Content;

            var mappedGenre = _mapper.Map<GenreDto>(genre);

            return Ok(mappedGenre);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);

            var genreResponse = await _genresService.SaveAsync(genre);

            var savedGenre = genreResponse.Content;

            var savedGenreMapped = _mapper.Map<GenreDto>(savedGenre);

            return Ok(savedGenreMapped);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(GenreUpsertDto genreDto, int id)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            genre.Id = id;

            var genreResponse = await _genresService.UpdateAsync(genre);

            if (!genreResponse.Success)
                return NotFound(genreResponse.Error);

            var updatedGenre = genreResponse.Content;

            var updatedGenreMapped = _mapper.Map<GenreDto>(updatedGenre);

            return Ok(updatedGenreMapped);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genreResponse = await _genresService.DeleteAsync(id);

            if (!genreResponse.Success)
                return NotFound(genreResponse.Error);

            var deletedGenre = genreResponse.Content;

            var deletedGenreMapped = _mapper.Map<GenreDto>(deletedGenre);

            return Ok(deletedGenreMapped);
        }
    }
}
