using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Services
{
    public interface IUsersService
    {
        public Task<string> Login(User user);
        public Task<User> SignUp(User user);
        public Task<User> GetAsync(int id);
    }
}
