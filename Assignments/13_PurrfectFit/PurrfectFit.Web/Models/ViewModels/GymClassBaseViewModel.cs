using System.ComponentModel.DataAnnotations;

namespace PurrfectFit.Web.Models.ViewModels
{
	public abstract class GymClassBaseViewModel
	{
		[Required]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "The name must be between {2} and {1} characters.")]
		public required string Name { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[Display(Name = "Start Time")]
		public DateTime StartTime { get; set; } = DateTime.Now.AddDays(1);

		[Required]
		[DataType(DataType.Duration)]
		[Display(Name = "Duration (Minutes)")]
		public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(60);

		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		public string? Description { get; set; }
	}
}
