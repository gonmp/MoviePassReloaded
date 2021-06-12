using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Movie> Movies { get; set; }
    }
}
