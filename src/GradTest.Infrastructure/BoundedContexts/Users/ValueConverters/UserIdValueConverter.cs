using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;

namespace GradTest.Infrastructure.BoundedContexts.Users.ValueConverters;

public class UserIdValueConverter() : ValueConverter<UserId, Guid>(
    userId => userId.Value,
    guid => UserId.FromGuid(guid)
);