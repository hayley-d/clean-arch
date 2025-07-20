namespace GradTest.Domain.Common.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public abstract class EntityBase: ISoftDeletable
{
    public DateTime DateCreated { get; private set; }
    public DateTime? DateModified { get; private set; }
    public Guid CreatedBy { get; private set; }
    public Guid? ModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public Guid? DeletedBy { get; private set; }
    
    public void Delete()
    {
        IsDeleted = true;
        DateModified = DateTime.UtcNow;
    }

    public void SetDeletedBy(Guid userId)
    {
        DeletedBy = userId;
    }

    public void SetCreatedBy(Guid userId)
    {
        CreatedBy = userId;
        DateCreated = DateTime.UtcNow;
    }

    public void SetModifiedBy(Guid userId)
    {
        ModifiedBy = userId;
        DateModified = DateTime.UtcNow;
    }
}