using GradTest.Domain.Common.Contracts;
using GradTest.Infrastructure.Common.Errors;
using GradTest.Infrastructure.Persistence;

namespace GradTest.Infrastructure.Common.Repository;

public class BaseRepository: IRepository
{
    protected readonly ApplicationDbContext Context;
    
    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await Context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Error(DatabaseError.Create(e.Message));
        }
    }
    
    public void Update<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Update(entity);
    }
    
    public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        Context.UpdateRange(entities);
    }
    
    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Remove(entity);
    }
    
    public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        Context.RemoveRange(entities);
    }
    
    public void Add<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Add(entity);
    }
    
    public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        Context.AddRange(entities);
    }
}