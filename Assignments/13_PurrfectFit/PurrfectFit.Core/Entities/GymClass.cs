using System.ComponentModel.DataAnnotations;

namespace PurrfectFit.Core.Entities
{
	public class GymClass
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "The name must be between {2} and {100} characters.")]
		public required string Name { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[Display(Name = "Start Time")]
		public DateTime StartTime { get; set; }

		[Required]
		[DataType(DataType.Duration)]
		public TimeSpan Duration { get; set; }

		[Display(Name = "End Time")]
		public DateTime EndTime => StartTime + Duration;

		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		public string? Description { get; set; }
		public ICollection<ApplicationUserGymClass> AttendingMembers { get; set; } = new List<ApplicationUserGymClass>();
	}
}
