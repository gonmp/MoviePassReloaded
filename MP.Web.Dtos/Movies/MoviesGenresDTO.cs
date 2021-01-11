using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Movies
{
    public class MoviesGenresDTO
    {
        public MovieDTO Movie { get; set; }
        public GenreDTO Genre { get; set; }
    }
}
