﻿using Dolphin_Book.Core.Constants;
using Dolphin_Book.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Service.Helpers
{
    public static class DbInitializer
    {
        public async static Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        private async static Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues<UserRoles>())
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }
            }
        }

        private async static Task SeedUsersAsync(UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync("Admin");
            if (user is null)
            {
                user = new User
                {
                    UserName = "Admin",
                    Fullname = "Admin",
                    Email = "admin@app.com"
                };

                var result = await userManager.CreateAsync(user, "Admin12345678*");
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        throw new Exception(error.Description);
                }

                await userManager.AddToRoleAsync(user, UserRoles.SuperAdmin.ToString());
            }
        }
    }
}

