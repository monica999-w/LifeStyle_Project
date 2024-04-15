using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;


namespace LifeStyle.Aplication.Logic
{
    public class UserRepository : IRepository<UserProfile>
    {
        private readonly List<UserProfile> _userProfiles = new List<UserProfile>();


        public UserRepository()
        {

            _userProfiles.Add(new UserProfile(1, "john@example.com", "123456789", 175, 70));
            _userProfiles.Add(new UserProfile(2, "jane@example.com", "987654321", 160, 55));
           
        }

        public async Task<IEnumerable<UserProfile>> GetAll()
        {

            await Task.Delay(0);
            return _userProfiles;
        }

        public async Task Add(UserProfile entity)
        {

            await Task.Delay(0);
            _userProfiles.Add(entity);
        }

        public async Task Remove(UserProfile entity)
        {

            await Task.Delay(0);
            var existingProfile = await GetById(entity.Id);
            if (existingProfile != null)
            {
                _userProfiles.Remove(existingProfile);
            }
            else
            {
                throw new KeyNotFoundException("User profile not found");
            }
        }

        public async Task Update(UserProfile entity)
        {

            await Task.Delay(0);
            var existingProfile = await GetById(entity.Id);
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
        }

        public async Task<UserProfile?> GetById(int id)
        {
            UserProfile? userProfile = await Task.FromResult(_userProfiles.FirstOrDefault(u => u.Id == id));
            if (userProfile == null)
            {
                throw new KeyNotFoundException("User profile not found");
            }
            return userProfile;
        }

        public int GetLastId()
        {
            if (_userProfiles.Any())
            {
                return _userProfiles.Max(m => m.Id);
            }
            else
            {
                return 0;
            }
        }
    }
}
