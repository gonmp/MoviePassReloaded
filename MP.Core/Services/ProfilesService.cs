using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Models;
using MP.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Profile = MP.Core.Models.Profile;

namespace MP.Core.Services
{
    public class ProfilesService : IProfilesService
    {
        private readonly DataAccessContext _dataContext;
        private readonly IMapper _mapper;

        public ProfilesService(DataAccessContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<Profile> DeleteAsync(int id)
        {
            var profileToDelete = await _dataContext.Genres.SingleOrDefaultAsync(p => p.Id == id);

            if (profileToDelete == null) return null;

            _dataContext.Remove(profileToDelete);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Profile>(profileToDelete);
        }

        public async Task<List<Profile>> GetAllAsync()
        {
            var profilesList = await _dataContext.Profiles.ToListAsync();

            return _mapper.Map<List<Profile>>(profilesList);
        }

        public async Task<Profile> GetAsync(int userId)
        {
            var profile = await _dataContext.Profiles.Include(p => p.User).ThenInclude(u => u.UserRol).SingleOrDefaultAsync(p => p.UserId == userId);

            return _mapper.Map<Profile>(profile);
        }

        public async Task<Profile> SaveAsync(int userId, Profile profile)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var mappedProfile = _mapper.Map<DataAccess.EntityModels.Profile>(profile);
            mappedProfile.UserId = userId;
            mappedProfile.User = user;

            await _dataContext.Profiles.AddAsync(mappedProfile);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Profile>(mappedProfile); ;
        }

        public async Task<Profile> UpdateAsync(Profile profile)
        {
            var result = await _dataContext.Profiles.Include(p => p.User).SingleOrDefaultAsync(p => p.Id == profile.Id);
            if (result == null)
            {
                throw new System.ArgumentException("There is no genre with Id = " + profile.Id, "profile.Id");
            }

            result.Name = profile.Name;
            result.LastName = profile.LastName;
            result.Dni = profile.Dni;

            await _dataContext.SaveChangesAsync();

            return _mapper.Map<Profile>(result);
        }
    }
}
