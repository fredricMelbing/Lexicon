using System.ComponentModel.DataAnnotations;

namespace PurrfectFit.Web.Models.ViewModels
{
	public class GymClassScheduleViewModel : GymClassBaseViewModel, IValidatableObject
	{		
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (StartTime < DateTime.Now)
			{
				yield return new ValidationResult(
					"The start time cannot be in the past when creating a new class.",
					new[] { nameof(StartTime) }
				);
			}
		}
	}
}
