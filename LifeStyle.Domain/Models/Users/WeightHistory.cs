using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Domain.Models.Users
{
    public class WeightHistory
    {
        [Key]
        public int Id { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
