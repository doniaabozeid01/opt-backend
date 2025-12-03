using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using optimum.data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {


        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var currentUtcDate = DateTime.UtcNow; // التأكد من استخدام نفس الوقت
            var expirationDate = DateTime.UtcNow.AddDays(30); // مدة التوكين ساعة بتوقيت UTC

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"])
        }),
                NotBefore = currentUtcDate, // إضافة NotBefore مع التوقيت نفسهn  





                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }























        public string? ValidateEmail(string email)
        {
            if (email.Contains(' '))
                return "The email format shouldn't have any spaces.";

            if (!email.Contains('@'))
                return "The email format is invalid.";

            var domain = email.Split('@').Last();

            if (domain != domain.ToLower())
                return "Invalid email format. The domain must be in lowercase.";

            if (!email.EndsWith("@gmail.com"))
                return "Only Gmail accounts are allowed.";

            return null; // يعني الإيميل سليم
        }

    }

}
