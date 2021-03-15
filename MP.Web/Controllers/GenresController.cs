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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("~/api/themoviesdb/genres")]
        public async Task<IActionResult> GetAllFromApiAsync()
        {
            var genres = await _genresService.GetAllFromApiAsync();

            var genresDto = _mapper.Map<List<GenreDTO>>(genres);

            return Ok(genresDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAync()
        {
            var genres = await _genresService.GetAllAsync();

            var genresDto = _mapper.Map<List<GenreDTO>>(genres);

            return Ok(genresDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var genre = await _genresService.GetAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GenreDTO>(genre));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(GenreDTO genreDto)
        {
            var genre = await _genresService.SaveAsync(_mapper.Map<Genre>(genreDto));

            return Ok(_mapper.Map<GenreDTO>(genre));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(GenreUpsertDTO genreDto, int id)
        {
            var genre = await _genresService.GetAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            _mapper.Map(genreDto, genre);

            var updatedGenre = _mapper.Map<GenreDTO>(await _genresService.UpdateAsync(genre));

            return Ok(updatedGenre);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _genresService.GetAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            var deletedGenre = _genresService.DeleteAsync(id);

            return Ok(_mapper.Map<GenreDTO>(deletedGenre));
        }
    }
}
