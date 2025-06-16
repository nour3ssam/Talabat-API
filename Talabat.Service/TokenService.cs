using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<string> CreateTokenasync(AppUser user, UserManager<AppUser>usermanger)
        {

            // 1. private Claim
            var AddClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.displayName),
                new Claim(ClaimTypes.Email,user.Email),

            };
            var UserRole = await usermanger.GetRolesAsync(user);
            foreach (var Role in UserRole)
            {
                AddClaim.Add(new Claim(ClaimTypes.Role, Role));
            }

            // 2. Key
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            // 3. Register Claim (issuer , audience , expires )
            var Token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
                claims : AddClaim,
                // 4 . Algorithm
                signingCredentials:new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)
                );
          
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }   
}
