using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Domain.Models.Users
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Height must be a positive number")]
        public double Height { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
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
