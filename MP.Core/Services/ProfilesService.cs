using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MP.Core.Models;
using MP.Core.Response;
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

        public async Task<ServiceResponse<List<Profile>>> GetAllAsync()
        {
            var profilesList = await _dataContext.Profiles.ToListAsync();

            var mappedProfilesList = _mapper.Map<List<Profile>>(profilesList);

            return new ServiceResponse<List<Profile>>(mappedProfilesList);
        }

        public async Task<ServiceResponse<Profile>> GetAsync(int userId)
        {
            var profile = await _dataContext.Profiles.Include(p => p.User).ThenInclude(u => u.UserRol).SingleOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return new ServiceResponse<Profile>(new Error(ErrorCodes.ProfileNotExists, ErrorMessages.ProfileNotExists(userId)));

            var mappedProfile = _mapper.Map<Profile>(profile);

            return new ServiceResponse<Profile>(mappedProfile);
        }

        public async Task<ServiceResponse<Profile>> SaveAsync(int userId, Profile profile)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var mappedProfile = _mapper.Map<DataAccess.EntityModels.Profile>(profile);
            mappedProfile.UserId = userId;
            mappedProfile.User = user;

            await _dataContext.Profiles.AddAsync(mappedProfile);
            await _dataContext.SaveChangesAsync();

            var savedProfile = _mapper.Map<Profile>(mappedProfile);

            return new ServiceResponse<Profile>(savedProfile);
        }

        public async Task<ServiceResponse<Profile>> UpdateAsync(Profile profile)
        {
            var result = await _dataContext.Profiles.Include(p => p.User).SingleOrDefaultAsync(p => p.Id == profile.Id);

            if (result == null)
                return new ServiceResponse<Profile>(new Error(ErrorCodes.ProfileNotExists, ErrorMessages.ProfileNotExists(profile.Id)));

            result.Name = profile.Name;
            result.LastName = profile.LastName;
            result.Dni = profile.Dni;

            await _dataContext.SaveChangesAsync();

            var updatedProfile = _mapper.Map<Profile>(result);

            return new ServiceResponse<Profile>(updatedProfile);
        }

        public async Task<ServiceResponse<Profile>> DeleteAsync(int id)
        {
            var profileToDelete = await _dataContext.Genres.SingleOrDefaultAsync(p => p.Id == id);

            if (profileToDelete == null)
                return new ServiceResponse<Profile>(new Error(ErrorCodes.ProfileNotExists, ErrorMessages.ProfileNotExists(id)));

            _dataContext.Remove(profileToDelete);
            await _dataContext.SaveChangesAsync();

            var deletedProfile = _mapper.Map<Profile>(profileToDelete);

            return new ServiceResponse<Profile>(deletedProfile);
        }
    }
}
