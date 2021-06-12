using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MP.DataAccess.EntityModels
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Image { get; set; }
        public string Overview { get; set; }
        public int Duration { get; set; }
        public IList<MoviesGenres> MoviesGenres { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Show> Shows { get; set; }
    }
}
