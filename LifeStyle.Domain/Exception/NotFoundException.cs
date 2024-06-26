﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Domain.Exception
{
    public class NotFoundException : DataValidationException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
