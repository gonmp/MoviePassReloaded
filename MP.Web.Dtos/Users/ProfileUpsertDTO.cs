﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Users
{
    public class ProfileUpsertDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Dni { get; set; }
    }
}
