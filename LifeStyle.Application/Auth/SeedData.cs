using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;


namespace LifeStyle.Application.Auth
{
    public class SeedDataService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedDataService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAdminUserAsync()
        {
            const string adminEmail = "admin@admin.com";
            const string adminPassword = "Admin@123";

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    var adminRole = await _roleManager.FindByNameAsync("Admin");
                    if (adminRole == null)
                    {
                        adminRole = new IdentityRole("Admin");
                        await _roleManager.CreateAsync(adminRole);
                    }

                    await _userManager.AddToRoleAsync(adminUser, "Admin");

                    var adminClaims = new List<Claim>
                {
                    new(ClaimTypes.Email, adminEmail),
                    new(ClaimTypes.Role, "Admin")
                };

                    await _userManager.AddClaimsAsync(adminUser, adminClaims);
                }
            }
        }
    }
}