using GradTest.Domain.BoundedContexts.Users.ValueObjects;

namespace GradTest.Domain.Common.Entities;

public interface IAuditable
{
    public DateTime DateCreated { get; }
    public DateTime? DateModified { get; }
    public UserId CreatedBy { get; }
    public UserId? ModifiedBy { get; }

    public void SetCreatedBy(UserId userId);
    public void SetModifiedBy(UserId userId);
}