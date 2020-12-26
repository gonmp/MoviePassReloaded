using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class GenresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenresService _genresService;

        public GenresController(IMapper mapper, IGenresService genresService)
        {
            _mapper = mapper;
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFromApiAsync()
        {
            var genres = await _genresService.GetAllFromApiAsync();

            var genresDto = _mapper.Map<List<GenreDTO>>(genres);

            return Ok(genresDto);
        }
    }
}
