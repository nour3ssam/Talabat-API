using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace TalabatAPI.Extensions
{
    public static  class userManagerExtension
    {
        public static async Task<AppUser> FindUserByAddressAsync(this UserManager<AppUser> usermanger, ClaimsPrincipal User)
        {
            var Email =  User.FindFirstValue(ClaimTypes.Email); 
            var user =await usermanger.Users.Where(U=>U.Email == Email).Include(U=>U.Address).FirstOrDefaultAsync();
            return user;

        }

    }
}
