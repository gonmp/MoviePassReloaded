using AutoMapper;
using MP.Core;
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
            CreateMap<Movie, MovieDTO>()
                .ReverseMap();
        }
    }
}
