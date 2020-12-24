using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Image { get; set; }
        public string Overview { get; set; }
        public int Duration { get; set; }
    }
}
