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
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public UsersController(IMapper mapper, IUsersService usersService)
        {
            _mapper = mapper;
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserUpsertDTO userDto)
        {
            var token = await _usersService.Login(_mapper.Map<User>(userDto));

            if(token == null)
            {
                return BadRequest();
            }

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserUpsertDTO userDto)
        {
            var newUser = await _usersService.SignUp(_mapper.Map<User>(userDto));

            if(newUser == null)
            {
                return BadRequest();
            }

            var newUserDto = _mapper.Map<UserDTO>(newUser);

            return Ok(newUserDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _usersService.GetAsync(Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
