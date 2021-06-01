using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Shows
{
    public class ShowUpsertDto
    {
        public DateTime DateTime { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
    }
}
