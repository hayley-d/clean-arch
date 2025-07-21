using GradTest.Shared.Monads;
namespace GradTest.Domain.Common.Contracts;

public interface IRepository
{
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    void Update<TEntity>(TEntity entity) where TEntity : class;

    void Delete<TEntity>(TEntity entity) where TEntity : class;
    
    void Add<TEntity>(TEntity entity) where TEntity : class;
}