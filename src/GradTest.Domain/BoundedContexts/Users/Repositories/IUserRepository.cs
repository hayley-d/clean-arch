using GradTest.Domain.BoundedContexts.Users.Entities;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;
using GradTest.Domain.Common.Contracts;

namespace GradTest.Domain.BoundedContexts.Users.Repositories;

public interface IUserRepository: IRepository
{
    public Task<User?> TryGetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
}