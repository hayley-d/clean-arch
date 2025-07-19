using GradTest.Domain.BoundedContexts.Users.ValueObjects;

namespace GradTest.Domain.Common.Entities;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    public UserId? DeletedBy { get; }
    public void Delete();
    public void SetDeletedBy(UserId userId);
}