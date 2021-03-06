using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUi1.Entities;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebUi1.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            for (int i = 0; i < 100; i++)
            {
                var regex = new Regex(Regex.Escape("xv"));
                 userData = regex.Replace(userData, i.ToString(), 1);
            }
            for (int i = 0; i < 100; i++)
            {
                var regex = new Regex(Regex.Escape("xt"));
                userData = regex.Replace(userData, i.ToString(), 1);
            }
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            var roles = new List<AppRole>
                {
                    new AppRole{Name = "Member"},
                    new AppRole{Name = "Admin"},
                    new AppRole{Name = "Moderator"},
                };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
            foreach (var user in users)
            {

                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "password");
                await userManager.AddToRoleAsync(user, "Member");


            }
            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "password");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }
        //public static async Task SeedUsers(UserManager<AppUser> userManager, 
        //    RoleManager<AppRole> roleManager)
        //{
        //    if (await userManager.Users.AnyAsync()) return;

        //    var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
        //    var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
        //    if (users == null) return;

        //    var roles = new List<AppRole>
        //    {
        //        new AppRole{Name = "Member"},
        //        new AppRole{Name = "Admin"},
        //        new AppRole{Name = "Moderator"},
        //    };

        //    foreach (var role in roles)
        //    {
        //        await roleManager.CreateAsync(role);
        //    }

        //    foreach (var user in users)
        //    {
        //        user.UserName = user.UserName.ToLower();
        //        await userManager.CreateAsync(user, "Pa$$w0rd");
        //        await userManager.AddToRoleAsync(user, "Member");
        //    }

        //    var admin = new AppUser
        //    {
        //        UserName = "admin"
        //    };

        //    await userManager.CreateAsync(admin, "Pa$$w0rd");
        //    await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
        //}
    }
}
