using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Movies
{
    public class MovieUpsertDto
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public string Image { get; set; }
        public string Overview { get; set; }
        public int Duration { get; set; }
        public List<GenreDto> Genres { get; set; }
    }
}
