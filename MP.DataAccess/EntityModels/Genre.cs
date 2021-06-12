using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MP.DataAccess.EntityModels
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<MoviesGenres> MoviesGenres { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
