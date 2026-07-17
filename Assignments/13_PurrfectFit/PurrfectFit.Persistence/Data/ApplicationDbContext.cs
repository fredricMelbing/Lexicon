using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PurrfectFit.Core.Entities;

namespace PurrfectFit.Persistence.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<GymClass> GymClasses { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{			
			base.OnModelCreating(builder);

			// This single line scans the current assembly for any classes
			// implementing IEntityTypeConfiguration and applies them automatically!
			builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		}
	}
}
