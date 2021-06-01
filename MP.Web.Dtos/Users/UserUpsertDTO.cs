using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Users
{
    public class UserUpsertDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
