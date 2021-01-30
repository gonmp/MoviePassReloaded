using MP.Web.Dtos.Movies;
using MP.Web.Dtos.Rooms;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Shows
{
    public class ShowDTO
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public MovieDTO Movie { get; set; }
        public RoomDTO Room { get; set; }
    }
}
