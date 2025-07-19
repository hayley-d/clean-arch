using GradTest.Shared.Monads;
namespace GradTest.Domain.Common.Contracts;

public interface IRepository
{
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    void Update<TEntity>(TEntity entity) where TEntity : class;
    void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    void Delete<TEntity>(TEntity entity) where TEntity : class;
    void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    
    void Add<TEntity>(TEntity entity) where TEntity : class;
    void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class; 
}