using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Interfaces;
using MP.Core.Models;
using MP.Web.Dtos.Shows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/shows")]
    public class ShowsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShowsService _showsService;

        public ShowsController(IMapper mapper, IShowsService showsService)
        {
            _mapper = mapper;
            _showsService = showsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var shows = await _showsService.GetAllAsync();

            var showsDto = _mapper.Map<List<ShowDTO>>(shows);

            return Ok(showsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var show = await _showsService.GetAsync(id);

            if (show == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ShowDTO>(show));
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(ShowUpsertDTO showDto)
        {
            var show = await _showsService.SaveAsync(_mapper.Map<Show>(showDto));

            return Ok(_mapper.Map<ShowDTO>(show));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(ShowUpsertDTO showDto, int id)
        {
            var show = await _showsService.GetAsync(id);

            if (show == null)
            {
                return NotFound();
            }

            _mapper.Map(showDto, show);

            var updatedShow = _mapper.Map<ShowDTO>(await _showsService.UpdateAsync(show));

            return Ok(updatedShow);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var show = await _showsService.GetAsync(id);

            if (show == null)
            {
                return NotFound();
            }

            var deletedShow = _showsService.DeleteAsync(id);

            return Ok(_mapper.Map<ShowDTO>(deletedShow));
        }
    }
}
