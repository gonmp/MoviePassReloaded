using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Web.Dtos.Cinemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/cinemas")]
    public class CinemasController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICinemasService _cinemasService;

        public CinemasController(IMapper mapper, ICinemasService cinemasService)
        {
            _mapper = mapper;
            _cinemasService = cinemasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var cinemasResponse = await _cinemasService.GetAllAsync();

            var cinemas = cinemasResponse.Content;

            var cinemasDto = _mapper.Map<List<CinemaDto>>(cinemas);

            return Ok(cinemasDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var cinemaResponse = await _cinemasService.GetAsync(id);

            if (!cinemaResponse.Success)
                return NotFound(cinemaResponse.Error);

            var cinema = cinemaResponse.Content;

            var mappedCinema = _mapper.Map<CinemaDto>(cinema);

            return Ok(mappedCinema);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(CinemaUpsertDto cinemaDto)
        {
            var cinema = _mapper.Map<Cinema>(cinemaDto);

            var cinemaResponse = await _cinemasService.SaveAsync(cinema);

            var savedCinema = cinemaResponse.Content;

            var savedCinemaDto = _mapper.Map<CinemaDto>(savedCinema);

            return Ok(savedCinemaDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(CinemaUpsertDto cinemaDto, int id)
        {
            var cinema = _mapper.Map<Cinema>(cinemaDto);
            cinema.Id = id;

            var cinemaResponse = await _cinemasService.UpdateAsync(cinema);

            if (!cinemaResponse.Success)
                return NotFound(cinemaResponse.Error);

            var updatedCinema = cinemaResponse.Content;

            var updatedCinemaDto = _mapper.Map<CinemaDto>(updatedCinema);

            return Ok(updatedCinemaDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var cinemaResponse = await _cinemasService.DeleteAsync(id);

            if (!cinemaResponse.Success)
                return NotFound(cinemaResponse.Error);

            var deletedCinema = cinemaResponse.Content;

            var deletedCinemaDto = _mapper.Map<CinemaDto>(deletedCinema);

            return Ok(deletedCinemaDto);
        }
    }
}
