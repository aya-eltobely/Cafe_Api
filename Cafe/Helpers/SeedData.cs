using Cafe.Models;
using Microsoft.AspNetCore.Identity;

namespace Cafe.Helpers
{
    public static class SeedData
    {

        public static void Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDBContext>();


            //create Roles
            if (!roleManager.RoleExistsAsync(WebSiteRoles.SiteAdmin).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(WebSiteRoles.SiteAdmin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(WebSiteRoles.SiteDelivery)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(WebSiteRoles.SiteUser)).GetAwaiter().GetResult();
            }

            //Create Admin
            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Aya",
                Email = "admin@gmail.com",
                FirstName ="Admin",
                LastName =""
            }, "Admin123#").GetAwaiter().GetResult();

            //check admin exist
            var AppAdmin = userManager.FindByEmailAsync("admin@gmail.com").GetAwaiter().GetResult();

            //asign role to admin
            if (AppAdmin != null)
            {
                userManager.AddToRoleAsync(AppAdmin, WebSiteRoles.SiteAdmin).GetAwaiter().GetResult();
            }

            //////////////    

            dbContext.SaveChanges();




        }

    }
}
