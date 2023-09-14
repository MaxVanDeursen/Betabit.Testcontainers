using Betatalks.Testcontainers.Core.Entities;
using Betatalks.Testcontainers.Infrastructure.Database.EntityConfigurations;
using Betatalks.Testcontainers.Infrastructure.Database.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace Betatalks.Testcontainers.Infrastructure.Database;


#pragma warning disable CS8618
public sealed class TestcontainersContext : DbContext
{
    public DbSet<User> Users { get; private set; }

    public TestcontainersContext(DbContextOptions options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder?.Properties<DateOnly>().HaveConversion<DateOnlyConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .ApplyConfiguration(new UserConfiguration());
    }
}
#pragma warning restore CS8618
