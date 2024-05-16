using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Domain
{
    public class InputValidator
    {
        public class EntityValidator<T>
        {
            public bool ValidateEntity(T entity)
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(entity, serviceProvider: null, items: null);
                return Validator.TryValidateObject(entity, context, validationResults, validateAllProperties: true);
            }
        }

    }
}
