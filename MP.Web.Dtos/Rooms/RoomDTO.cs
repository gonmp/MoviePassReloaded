using MP.Web.Dtos.Cinemas;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Rooms
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int TicketValue { get; set; }
        public CinemaDto Cinema { get; set; }
    }
}
