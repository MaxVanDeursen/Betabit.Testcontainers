using Betatalks.Testcontainers.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betatalks.Testcontainers.Infrastructure.Database.EntityConfigurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder = builder ?? throw new ArgumentNullException(nameof(builder));
        builder.Property(user => user.DateOfBirth).HasColumnType("date");
    }
}
