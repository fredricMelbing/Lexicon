namespace PurrfectFit.Core.Entities
{
	public class ApplicationUserGymClass
	{		
		public required string ApplicationUserId { get; set; }
		public required int GymClassId { get; set; }		
		public ApplicationUser? ApplicationUser { get; set; }
		public GymClass? GymClass { get; set; }
	}
}
