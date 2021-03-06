﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int TicketValue { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
    }
}
