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

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> AdminGet(int id)
        {
            var profileResponse = await _profilesService.GetAsync(id);

            if (!profileResponse.Success)
                return NotFound(profileResponse.Error);

            var profile = profileResponse.Content;

            var profileDto = _mapper.Map<ProfileDto>(profile);

            return Ok(profileDto);
        }

        [Authorize]
        [HttpGet("current_user")]
        public async Task<IActionResult> Get()
        {
            var profileResponse = await _profilesService.GetAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value));

            if (!profileResponse.Success)
                return NotFound(profileResponse.Error);

            var profile = profileResponse.Content;

            var profileDto = _mapper.Map<ProfileDto>(profile);

            return Ok(profileDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profilesResponse = await _profilesService.GetAllAsync();

            var profiles = profilesResponse.Content;

            var profilesDto = _mapper.Map<List<ProfileDto>>(profiles);

            return Ok(profilesDto);
        }

        [Authorize]
        [HttpPost("current_user")]
        public async Task<IActionResult> SaveAsync(ProfileUpsertDto profileDto)
        {
            var profile = _mapper.Map<Profile>(profileDto);

            var profileResponse = await _profilesService.SaveAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value), profile);

            var savedProfile = profileResponse.Content;

            var savedProfileDto = _mapper.Map<ProfileDto>(savedProfile);

            return Ok(savedProfileDto);
        }
    }
}
