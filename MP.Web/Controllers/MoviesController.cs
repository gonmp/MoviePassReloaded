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
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMoviesService _moviesService;

        public MoviesController(IMapper mapper, IMoviesService moviesService)
        {
            _mapper = mapper;
            _moviesService = moviesService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("~/api/themoviesdb/movies")]
        public async Task<IActionResult> GetAllFromApiAsync()
        {
            var movies = await _moviesService.GetAllFromApiAsync();

            var moviesDto = _mapper.Map<List<MovieDto>>(movies);

            return Ok(moviesDto);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetAync(string movieTitle, int pageSize, int pageNumber)
        {
            var moviesResponse = await _moviesService.GetAsync(movieTitle, pageSize, pageNumber);

            var movies = moviesResponse.Content;

            var moviesDto = _mapper.Map<List<MovieDto>>(movies);

            return Ok(moviesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var movieResponse = await _moviesService.GetAsync(id);

            if (!movieResponse.Success)
                return NotFound(movieResponse.Error);

            var movie = movieResponse.Content;

            var mappedMovie = _mapper.Map<MovieDto>(movie);

            return Ok(mappedMovie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(MovieDto movieDto)
        {
            var movieResponse = await _moviesService.SaveAsync(_mapper.Map<Movie>(movieDto));

            if (!movieResponse.Success)
                return NotFound(movieResponse.Error);

            var movie = movieResponse.Content;

            var mappedMovie = _mapper.Map<MovieDto>(movie);

            return Ok(mappedMovie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(MovieUpsertDto movieDto, int id)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            movie.Id = id;

            var movieResponse = await _moviesService.UpdateAsync(movie);

            if (!movieResponse.Success)
                return NotFound(movieResponse.Error);

            var updatedMovie = movieResponse.Content;

            var updatedMovieDto = _mapper.Map<MovieDto>(updatedMovie);

            return Ok(updatedMovieDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movieResponse = await _moviesService.DeleteAsync(id);

            if (!movieResponse.Success)
                return NotFound(movieResponse.Error);

            var deletedMovie = movieResponse.Content;

            var deletedMovieDto = _mapper.Map<MovieDto>(deletedMovie);

            return Ok(deletedMovieDto);
        }
    }
}
