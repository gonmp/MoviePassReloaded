using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Models;
using MP.Core.Services;
using MP.Web.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MP.Web.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public UsersController(IMapper mapper, IUsersService usersService)
        {
            _mapper = mapper;
            _usersService = usersService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserUpsertDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var tokenResponse = await _usersService.Login(user);

            if (!tokenResponse.Success)
                return BadRequest(tokenResponse.Error);

            var token = tokenResponse.Content;

            return Ok(token);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserUpsertDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var newUserResponse = await _usersService.SignUp(user);

            if (!newUserResponse.Success)
                return BadRequest(newUserResponse.Error);

            var newUser = newUserResponse.Content;

            var newUserDto = _mapper.Map<UserDto>(newUser);

            return Ok(newUserDto);
        }

        [Authorize]
        [HttpGet("current_user")]
        public async Task<IActionResult> Get()
        {
            var userResponse = await _usersService.GetAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value));

            if (!userResponse.Success)
                return NotFound(userResponse.Error);

            var user = userResponse.Content;

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }
    }
}
