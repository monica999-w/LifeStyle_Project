using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Auth
{
    public class AuthResultDto
    {
        public string Token { get; set; }

        public AuthResultDto(string token)
        {
            Token = token;
        }
    }
}
