using LifeStyle.Domain.Models.Exercises;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Domain.Enums;

namespace LifeStyle.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.Property(e => e.Gender)
               .HasConversion(
                   v => v.ToString(),
                   v => (Gender)Enum.Parse(typeof(Gender), v));
        }
    }
    
}
