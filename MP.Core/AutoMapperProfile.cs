using AutoMapper;
using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DataAccess.EntityModels.Movie, Movie>()
                .ReverseMap();
            CreateMap<DataAccess.EntityModels.Genre, Genre>()
               .ReverseMap();
            CreateMap<DataAccess.EntityModels.MoviesGenres, MoviesGenres>()
               .ReverseMap();

        }
    }
}
