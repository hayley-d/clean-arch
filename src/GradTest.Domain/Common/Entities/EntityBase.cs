using System.ComponentModel.DataAnnotations.Schema;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;
using GradTest.Domain.Common.DomainEvents;

namespace GradTest.Domain.Common.Entities;

public abstract class EntityBase: IEntityBase, IAuditable, ISoftDeletable
{
    [NotMapped]
    private List<DomainEventBase> InternalDomainEvents { get; } = [];
    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => InternalDomainEvents;
    public void AddDomainEvent(DomainEventBase domainEvent) => InternalDomainEvents.Add(domainEvent);
    public void ClearDomainEvents() => InternalDomainEvents.Clear();
    
    public DateTime DateCreated { get; private set; }
    public DateTime? DateModified { get; private set; }
    public UserId CreatedBy { get; private set; }
    public UserId? ModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public UserId? DeletedBy { get; private set; }
    
    public void Delete()
    {
        IsDeleted = true;
        DateModified = DateTime.UtcNow;
    }

    public void SetDeletedBy(UserId userId) => DeletedBy = userId;
    public void SetCreatedBy(UserId userId)
    {
        CreatedBy = userId;
        DateCreated = DateTime.UtcNow;
    }

    public void SetModifiedBy(UserId userId)
    {
        ModifiedBy = userId;
        DateModified = DateTime.UtcNow;
    }
}