using AutoMapper;
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
        }
    }
}
