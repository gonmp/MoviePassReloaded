using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Services;
using MP.Web.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Profile = MP.Core.Models.Profile;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    public class ProfilesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProfilesService _profilesService;
        private readonly IUsersService _userService;

        public ProfilesController(IMapper mapper, IProfilesService profilesService, IUsersService userService)
        {
            _mapper = mapper;
            _profilesService = profilesService;
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> AdminGet(int id)
        {
            var profile = await _profilesService.GetAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProfileDTO>(profile));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("current_user")]
        public async Task<IActionResult> Get()
        {
            var profile = await _profilesService.GetAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value));

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProfileDTO>(profile));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles = await _profilesService.GetAllAsync();

            return Ok(_mapper.Map<List<ProfileDTO>>(profiles));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("current_user")]
        public async Task<IActionResult> SaveAsync(ProfileUpsertDTO profileDto)
        {
            var profile = await _profilesService.SaveAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value), _mapper.Map<Profile>(profileDto));

            return Ok(_mapper.Map<ProfileDTO>(profile));
        }
    }
}
