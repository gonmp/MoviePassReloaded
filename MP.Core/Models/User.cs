using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserRolId { get; set; }
        public UserRol UserRol { get; set; }
    }
}
