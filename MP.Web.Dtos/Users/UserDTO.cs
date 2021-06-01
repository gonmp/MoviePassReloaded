using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserRolId { get; set; }
        public UserRolDto UserRol { get; set; }
    }
}
