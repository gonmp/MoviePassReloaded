using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MP.Core.Models;
using MP.Core.Response;
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

        public async Task<ServiceResponse<string>> Login(User user)
        {
            var userDb = await _dataContext.Users.Include(u => u.UserRol).SingleOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

            if (userDb == null)
                return new ServiceResponse<string>(new Error(ErrorCodes.InvalidCredentials, ErrorMessages.InvalidCredentials));

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

            return new ServiceResponse<string>(tokenString);
        }

        public async Task<ServiceResponse<User>> SignUp(User user)
        {
            var userDb = await _dataContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);

            if (userDb != null)
                return new ServiceResponse<User>(new Error(ErrorCodes.UserAlreadyExists, ErrorMessages.UserAlreadyExists));

            user.UserRolId = 2;

            var mappedUser = _mapper.Map<DataAccess.EntityModels.User>(user);
            await _dataContext.Users.AddAsync(mappedUser);
            await _dataContext.SaveChangesAsync();

            var userFromDb = _mapper.Map<User>(mappedUser);

            return new ServiceResponse<User>(userFromDb);
        }

        public async Task<ServiceResponse<User>> GetAsync(int id)
        {
            var user = await _dataContext.Users.Include(u => u.UserRol).SingleOrDefaultAsync(u => u.Id == id);

            var mappedUser = _mapper.Map<User>(user);

            return new ServiceResponse<User>(mappedUser);
        }
    }
}
