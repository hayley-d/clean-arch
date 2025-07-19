using Microsoft.EntityFrameworkCore;
using GradTest.Domain.BoundedContexts.Users.Entities;
using GradTest.Domain.BoundedContexts.Users.Repositories;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;
using GradTest.Infrastructure.Common.Repository;
using GradTest.Infrastructure.Persistence;

namespace GradTest.Infrastructure.BoundedContexts.Users.Repositories;

public class UserRepository: BaseRepository, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }
    
    public Task<User?> TryGetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return Context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}