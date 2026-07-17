using Microsoft.AspNetCore.Identity;

namespace PurrfectFit.Core.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public ICollection<ApplicationUserGymClass> BookedClasses { get; set; } = new List<ApplicationUserGymClass>();
	}
}
