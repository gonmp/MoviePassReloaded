using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Rooms
{
    public class RoomUpsertDTO
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int TicketValue { get; set; }
        public int CinemaId { get; set; }
    }
}
