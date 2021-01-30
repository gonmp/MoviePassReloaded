using AutoMapper;
using MP.Core;
using MP.Core.Models;
using MP.Web.Dtos.Movies;
using MP.Web.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Profile = MP.Core.Models.Profile;

namespace MP.Web.Dtos
{
    public class AutoMapperProfile : AutoMapper.Profile
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
            CreateMap<User, UserDTO>();
            CreateMap<UserUpsertDTO, User>();
            CreateMap<UserRol, UserRolDTO>()
                .ReverseMap();
            CreateMap<Profile, ProfileDTO>();
            CreateMap<ProfileUpsertDTO, Profile>();
        }
    }
}
