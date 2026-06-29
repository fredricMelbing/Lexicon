using Microsoft.EntityFrameworkCore;

public class LogisticsSystemContext(DbContextOptions<LogisticsSystemContext> options) : DbContext(options)
{
    public DbSet<LogisticsSystem.Web.Models.Product> Product { get; set; } = default!;
}
