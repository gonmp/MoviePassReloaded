using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            var showsResponse = await _showsService.GetAllAsync();

            var shows = showsResponse.Content;

            var showsDto = _mapper.Map<List<ShowDto>>(shows);

            return Ok(showsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var showResponse = await _showsService.GetAsync(id);

            if (!showResponse.Success)
                return NotFound(showResponse.Error);

            var show = showResponse.Content;

            var showDto = _mapper.Map<ShowDto>(show);

            return Ok(showDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(ShowUpsertDto showDto)
        {
            var show = _mapper.Map<Show>(showDto);

            var showResponse = await _showsService.SaveAsync(show);

            if (!showResponse.Success)
                return NotFound(showResponse.Error);

            var savedShow = showResponse.Content;

            var savedShowDto = _mapper.Map<ShowDto>(savedShow);

            return Ok(savedShowDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(ShowUpsertDto showDto, int id)
        {
            var show = _mapper.Map<Show>(showDto);
            show.Id = id;

            var showResponse = await _showsService.UpdateAsync(show);

            if (!showResponse.Success)
                return NotFound(showResponse.Error);

            var updatedShow = showResponse.Content;

            var updatedShowDto = _mapper.Map<ShowDto>(updatedShow);

            return Ok(updatedShowDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var showResponse = await _showsService.DeleteAsync(id);

            if (!showResponse.Success)
                return NotFound(showResponse.Error);

            var deletedShow = showResponse.Content;

            var deletedShowDto = _mapper.Map<ShowDto>(deletedShow);

            return Ok(deletedShowDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowsListingsAsync()
        {
            var showsResponse = await _showsService.GetShowsListingsAsync();

            var shows = showsResponse.Content;

            var showsDto = _mapper.Map<List<ShowDto>>(shows);

            return Ok(showsDto);
        }
    }
}
