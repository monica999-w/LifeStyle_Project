using LifeStyle.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Responses
{
    public class UserDto
    {
        public int Id { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public required string PhoneNumber { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Height must be a positive number")]
        public double Height { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
        public double Weight { get; set; }


        public static UserDto FromUser(UserProfile userProfile)
        {
            return new UserDto
            {
                Id = userProfile.ProfileId,
                Email = userProfile.Email,
                PhoneNumber = userProfile.PhoneNumber,
                Height = userProfile.Height,
                Weight = userProfile.Weight

            };
        }

        public static UserProfile FromUserDto(UserDto userDto)
        {
            return new UserProfile
                (
                userDto.Id,
                userDto.Email,
                userDto.PhoneNumber,
                userDto.Height,
                userDto.Weight
                );
        }

    }
}
