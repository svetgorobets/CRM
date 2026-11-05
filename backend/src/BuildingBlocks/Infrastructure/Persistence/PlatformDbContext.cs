using Microsoft.EntityFrameworkCore;

namespace NotificationManagement.BuildingBlocks.Infrastructure.Persistence;

public sealed class PlatformDbContext(DbContextOptions<PlatformDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Each module adds only its own mappings here as that module is implemented.
        // This keeps a single PostgreSQL database without sharing domain persistence.
    }
}
