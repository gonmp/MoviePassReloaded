using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Users
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Dni { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
    }
}
