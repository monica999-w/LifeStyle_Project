using LifeStyle.Models.Planner;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Domain.Models.Users
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<Planner>Planners { get; set; } = new List<Planner>();

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
