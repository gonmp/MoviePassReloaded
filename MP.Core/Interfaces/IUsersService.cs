using MP.Core.Models;
using MP.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IUsersService
    {
        public Task<ServiceResponse<string>> Login(User user);
        public Task<ServiceResponse<User>> SignUp(User user);
        public Task<ServiceResponse<User>> GetAsync(int id);
    }
}
