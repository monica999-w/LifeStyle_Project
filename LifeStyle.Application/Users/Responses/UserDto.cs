using LifeStyle.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Responses
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public double Height { get; set; }
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
