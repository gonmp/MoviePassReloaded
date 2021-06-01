using MP.Web.Dtos.Movies;
using MP.Web.Dtos.Rooms;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Shows
{
    public class ShowDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public MovieDto Movie { get; set; }
        public RoomDto Room { get; set; }
    }
}
