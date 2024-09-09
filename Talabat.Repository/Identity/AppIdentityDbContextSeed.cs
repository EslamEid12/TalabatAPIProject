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
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Count()==0) 
            {
                var User = new AppUser()
                {
                    DisplayName = "Ahmed Nasr",
                    Email = "ahmed.nasr@linkdev.com",
                    UserName = "ahmed.nasr",
                    PhoneNumber = "01122334455"
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }

        }
    }
}
