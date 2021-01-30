using System;
using System.Collections.Generic;
using System.Text;

namespace MP.DataAccess.EntityModels
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Dni { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
