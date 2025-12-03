using optimum.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(ApplicationUser user);
        string? ValidateEmail(string email);


    }
}
