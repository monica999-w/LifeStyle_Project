using LifeStyle.Models.Planner;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Domain.Models.Users
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; } 
        public double Height { get; set; }
        public double Weight { get; set; }

        public UserProfile()
        {
        }

        public UserProfile(int id, string email, string phoneNumber, double height, double weight)
        {
            ProfileId = id;
            Email = email;
            PhoneNumber = phoneNumber;
            Height = height;
            Weight = weight;
        }
    }

}
