using AutoMapper;
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
    [Route("api/[controller]/[action]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMoviesService _moviesService;

        public MoviesController(IMapper mapper, IMoviesService moviesService)
        {
            _mapper = mapper;
            _moviesService = moviesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFromApiAsync()
        {
            var movies = await _moviesService.GetAllFromApiAsync();

            var moviesDto = _mapper.Map<List<MovieDTO>>(movies);

            return Ok(moviesDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAync()
        {
            var movies = await _moviesService.GetAllAsync();
            
            var moviesDto = _mapper.Map<List<MovieDTO>>(movies);

            return Ok(moviesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var movie = await _moviesService.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MovieDTO>(movie));
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(MovieDTO movieDto)
        {
            var movie = await _moviesService.SaveAsync(_mapper.Map<Movie>(movieDto));

            return Ok(_mapper.Map<MovieDTO>(movie));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(MovieUpsertDTO movieDto, int id)
        {
            var movie = await _moviesService.GetAsync(id);

            if(movie == null)
            {
                return NotFound();
            }

            _mapper.Map(movieDto, movie);

            var updatedMovie = _mapper.Map<MovieDTO>(await _moviesService.UpdateAsync(movie));

            return Ok(updatedMovie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _moviesService.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var deletedMovie = _moviesService.DeleteAsync(id);

            return Ok(_mapper.Map<MovieDTO>(deletedMovie));
        }
    }
}
