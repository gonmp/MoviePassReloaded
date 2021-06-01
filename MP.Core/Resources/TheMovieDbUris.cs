using MP.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Resources
{
    public static class TheMovieDbUris
    {
        public static string GetMovie(int id, string apiKey) => $"https://api.themoviedb.org/3/movie/{id}?api_key={apiKey}=en-US";

        public static string GetMoviesNowPlaying(int pageNumber, string apiKey) => $"https://api.themoviedb.org/3/movie/now_playing?api_key={apiKey}&language=es-AR&page={pageNumber}";

        public static string GetGenres(string apiKey) => $"https://api.themoviedb.org/3/genre/movie/list?api_key={apiKey}&language=es-ES";
    }
}
