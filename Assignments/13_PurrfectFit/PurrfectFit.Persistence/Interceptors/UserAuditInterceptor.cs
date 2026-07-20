using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PurrfectFit.Core.Entities;

namespace PurrfectFit.Persistence.Interceptors
{
	// Enterprise Pattern: A reusable interceptor that hooks into the EF Core SaveChanges pipeline
	public class UserAuditInterceptor : SaveChangesInterceptor
	{
		// Synchronous interceptor point
		public override InterceptionResult<int> SavingChanges(
			DbContextEventData eventData,
			InterceptionResult<int> result)
		{
			UpdateRegistrationTime(eventData.Context);
			return base.SavingChanges(eventData, result);
		}

		// Asynchronous interceptor point
		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
			DbContextEventData eventData,
			InterceptionResult<int> result,
			CancellationToken cancellationToken = default)
		{
			UpdateRegistrationTime(eventData.Context);
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		private static void UpdateRegistrationTime(DbContext? context)
		{
			if (context == null) return;

			// Scan EF Core's ChangeTracker for newly added ApplicationUser entities
			var addedUsers = context.ChangeTracker.Entries<ApplicationUser>()
				.Where(e => e.State == EntityState.Added);

			foreach (var entry in addedUsers)
			{
				// Enforce business rule: Automatically stamp registration time at database level
				entry.Entity.TimeOfRegistration = DateTime.Now;
			}
		}
	}
}
