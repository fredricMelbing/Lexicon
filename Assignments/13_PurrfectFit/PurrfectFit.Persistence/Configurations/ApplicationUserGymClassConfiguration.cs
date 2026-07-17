using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurrfectFit.Core.Entities;

namespace PurrfectFit.Persistence.Configurations
{
	public class ApplicationUserGymClassConfiguration : IEntityTypeConfiguration<ApplicationUserGymClass>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserGymClass> builder)
		{			
			builder.HasKey(ag => new { ag.ApplicationUserId, ag.GymClassId });
						
			builder.HasOne(ag => ag.ApplicationUser)
				.WithMany(u => u.BookedClasses)
				.HasForeignKey(ag => ag.ApplicationUserId);

			builder.HasOne(ag => ag.GymClass)
				.WithMany(g => g.AttendingMembers)
				.HasForeignKey(ag => ag.GymClassId);
		}
	}
}
