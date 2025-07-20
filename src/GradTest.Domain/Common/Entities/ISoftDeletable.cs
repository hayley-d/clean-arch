namespace GradTest.Domain.Common.Entities;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    public Guid? DeletedBy { get; }
    public void Delete();
    public void SetDeletedBy(Guid userId);
}