using AutoMapper;
using MP.Core;
using MP.Core.Models;
using MP.Web.Dtos.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MoviesGenres, MoviesGenresDTO>()
                .ReverseMap();
            CreateMap<Genre, GenreDTO>()
                .ReverseMap();
            CreateMap<Movie, MovieDTO>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MoviesGenres))
                .ReverseMap();            
        }
    }
}
