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
            var cinemas = await _cinemasService.GetAllAsync();

            var cinemasDto = _mapper.Map<List<CinemaDTO>>(cinemas);

            return Ok(cinemasDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var cinema = await _cinemasService.GetAsync(id);

            if (cinema == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CinemaDTO>(cinema));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(CinemaUpsertDTO cinemaDto)
        {
            var cinema = await _cinemasService.SaveAsync(_mapper.Map<Cinema>(cinemaDto));

            return Ok(_mapper.Map<CinemaDTO>(cinema));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(CinemaUpsertDTO cinemaDto, int id)
        {
            var cinema = await _cinemasService.GetAsync(id);

            if (cinema == null)
            {
                return NotFound();
            }

            _mapper.Map(cinemaDto, cinema);

            var updatedCinema = _mapper.Map<CinemaDTO>(await _cinemasService.UpdateAsync(cinema));

            return Ok(updatedCinema);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var cinema = await _cinemasService.GetAsync(id);

            if (cinema == null)
            {
                return NotFound();
            }

            var deletedCinema = _cinemasService.DeleteAsync(id);

            return Ok(_mapper.Map<CinemaDTO>(deletedCinema));
        }
    }
}
