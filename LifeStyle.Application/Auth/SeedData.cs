using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace LifeStyle.Application.Auth
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleNames = Enum.GetNames(typeof(RoleEnum));
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = new IdentityUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com"
            };

            string adminPassword = "Admin@123";

            var _user = await userManager.FindByEmailAsync("admin@admin.com");

            if (_user == null)
            {
                var createAdminUser = await userManager.CreateAsync(user, adminPassword);
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RoleEnum.Admin.ToString());
                }
            }
        }
    }
}