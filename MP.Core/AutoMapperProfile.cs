using AutoMapper;
using MP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Profile = MP.Core.Models.Profile;

namespace MP.Core
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DataAccess.EntityModels.Movie, Movie>()
                .ReverseMap();
            CreateMap<DataAccess.EntityModels.Genre, Genre>()
               .ReverseMap();
            CreateMap<DataAccess.EntityModels.MoviesGenres, MoviesGenres>()
               .ReverseMap();
            CreateMap<DataAccess.EntityModels.User, User>()
                .ReverseMap();
            CreateMap<DataAccess.EntityModels.UserRol, UserRol>()
                .ReverseMap();
            CreateMap<DataAccess.EntityModels.Profile, Profile>()
                .ReverseMap();
        }
    }
}
