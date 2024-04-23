using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace LifeStyle.Aplication.Logic
{
    public class UserRepository : IRepository<UserProfile>
    {
        private readonly LifeStyleContext _lifeStyleContext;



        public UserRepository(LifeStyleContext lifeStyleContext)
        {

          _lifeStyleContext = lifeStyleContext;
           
        }

        public async Task<IEnumerable<UserProfile>> GetAll()
        {
            return await _lifeStyleContext.UserProfiles
                .ToListAsync();
        }

        public async Task<UserProfile> Add(UserProfile entity)
        {

            _lifeStyleContext.UserProfiles.Add(entity);
            await _lifeStyleContext.SaveChangesAsync();
            return entity;
        }

        public async Task<UserProfile> Remove(UserProfile entity)
        {

            await Task.Delay(0);
            var existingProfile = await GetById(entity.ProfileId);
            if (existingProfile != null)
            {
                _lifeStyleContext.UserProfiles.Remove(existingProfile);
            }
            else
            {
                throw new Exception("User profile not found");
            }
            return entity;
        }

        public async Task<UserProfile> Update(UserProfile entity)
        {

            await Task.Delay(0);
            var existingProfile = await GetById(entity.ProfileId);
            if (existingProfile != null)
            {
                existingProfile.Email = entity.Email;
                existingProfile.PhoneNumber = entity.PhoneNumber;
                existingProfile.Height = entity.Height;
                existingProfile.Weight = entity.Weight;
            }
            else
            {
                throw new KeyNotFoundException("User profile not found");
            }
            return entity;  
        }

        public async Task<UserProfile?> GetById(int id)
        {
            UserProfile? userProfile = await Task.FromResult(_lifeStyleContext.UserProfiles.FirstOrDefault(u => u.ProfileId == id));
            if (userProfile == null)
            {
                throw new Exception("User profile not found");
            }
            return userProfile;
        }

        public int GetLastId()
        {
            if (_lifeStyleContext.UserProfiles.Any())
            {
                return _lifeStyleContext.UserProfiles.Max(m => m.ProfileId);
            }
            else
            {
                return 0;
            }
        }

        public async Task<UserProfile> GetByName(string name)
        {
            var user = await _lifeStyleContext.UserProfiles
                 .FirstOrDefaultAsync(e => e.Email == name);

            return user;
        }
    }
}
