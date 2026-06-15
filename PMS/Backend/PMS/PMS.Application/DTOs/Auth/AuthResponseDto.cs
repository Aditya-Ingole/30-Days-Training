using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
