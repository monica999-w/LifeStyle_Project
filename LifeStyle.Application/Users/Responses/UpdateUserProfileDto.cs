using LifeStyle.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Users.Responses
{
    public class UpdateUserProfileDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Height must be a positive number")]
        public double Height { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
        public double Weight { get; set; }

        public IFormFile? PhotoUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public Gender Gender { get; set; }
    }
}
