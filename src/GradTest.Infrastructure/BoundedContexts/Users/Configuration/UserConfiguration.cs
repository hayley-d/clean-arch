using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GradTest.Domain.BoundedContexts.Users.Entities;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;
using GradTest.Infrastructure.BoundedContexts.Users.ValueConverters;

namespace GradTest.Infrastructure.BoundedContexts.Users.Configuration;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User).ToLower(), nameof(Users).ToLower());
        
        builder
            .Property(x => x.Id)
            .HasConversion<UserIdValueConverter>();
    }
}