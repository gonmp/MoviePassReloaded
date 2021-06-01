using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Movies
{
    public class MoviesGenresDto
    {
        public MovieDto Movie { get; set; }
        public GenreDto Genre { get; set; }
    }
}
