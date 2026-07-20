using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PurrfectFit.Core.Entities
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between {2} and {1} characters.")]
		public required string FirstName { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between {2} and {1} characters.")]
		public required string LastName { get; set; }

		public string FullName => $"{FirstName} {LastName}";

		[Required]
		public DateTime TimeOfRegistration { get; set; }
		public ICollection<ApplicationUserGymClass> BookedClasses { get; set; } = new List<ApplicationUserGymClass>();
	}
}
