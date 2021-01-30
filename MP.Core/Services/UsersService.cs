using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MP.Core.Models;
using MP.DataAccess;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public UsersService(DataAccessContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<string> Login(User user)
        {
            var userDb = await _dataContext.Users.Include(u => u.UserRol).SingleOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
            if (userDb == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Nosequeponerasiquepongoesto");

            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDb.Id.ToString()),
                new Claim(ClaimTypes.Role, userDb.UserRol.Description),
                new Claim(ClaimTypes.Email, userDb.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<User> SignUp(User user)
        {
            var userDb = await _dataContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            if (userDb != null) return null;

            user.UserRolId = 2;

            var mappedUser = _mapper.Map<DataAccess.EntityModels.User>(user);
            await _dataContext.Users.AddAsync(mappedUser);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<User>(mappedUser);
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await _dataContext.Users.Include(u => u.UserRol).SingleOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<User>(user);
        }
    }
}
