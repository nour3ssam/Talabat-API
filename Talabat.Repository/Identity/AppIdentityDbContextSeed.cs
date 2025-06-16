using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public async static Task SeedUserAsync(UserManager<AppUser>userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    displayName = "Nour Essam",
                    Email = "nouressam1123@gmail.com",
                    UserName = "nouressam.Al3w",
                    PhoneNumber = "01015111214",
                };
                await userManager.CreateAsync(user, "01015Pp##$$qw");
            }


        }
    }
}
