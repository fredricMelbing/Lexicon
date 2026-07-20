using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PurrfectFit.Core.Entities;

namespace PurrfectFit.Persistence.Data
{
	public static class ApplicationDbSeed
	{
		public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
		{			
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
						
			string[] roleNames = { "Admin", "Member" };

			foreach (var roleName in roleNames)
			{
				var roleExist = await roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
						
			var adminEmail = "admin@admin.se";
			var defaultAdmin = await userManager.FindByEmailAsync(adminEmail);

			if (defaultAdmin == null)
			{
				var newAdmin = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true					
				};
								
				var createAdminResult = await userManager.CreateAsync(newAdmin, "password");

				if (createAdminResult.Succeeded)
					await userManager.AddToRoleAsync(newAdmin, "Admin");
			}
		}
	}
}
