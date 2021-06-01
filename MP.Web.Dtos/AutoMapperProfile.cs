using AutoMapper;
using MP.Core;
using MP.Core.Models;
using MP.Web.Dtos.Cinemas;
using MP.Web.Dtos.Movies;
using MP.Web.Dtos.Purchases;
using MP.Web.Dtos.Rooms;
using MP.Web.Dtos.Shows;
using MP.Web.Dtos.Tickets;
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
            CreateMap<MoviesGenres, MoviesGenresDto>()
                .ReverseMap();
            CreateMap<Genre, GenreDto>()
                .ReverseMap();
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MoviesGenres))
                .ReverseMap();
            CreateMap<User, UserDto>();
            CreateMap<UserUpsertDto, User>();
            CreateMap<UserRol, UserRolDto>()
                .ReverseMap();
            CreateMap<Profile, ProfileDto>();
            CreateMap<ProfileUpsertDto, Profile>();
            CreateMap<Cinema, CinemaDto>();
            CreateMap<CinemaUpsertDto, Cinema>();
            CreateMap<Room, RoomDto>();
            CreateMap<RoomUpsertDto, Room>();
            CreateMap<Show, ShowDto>();
            CreateMap<ShowUpsertDto, Show>();
            CreateMap<Purchase, PurchaseDto>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketUpsertDto, Ticket>();
            
        }
    }
}
