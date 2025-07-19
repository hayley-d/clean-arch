using Microsoft.EntityFrameworkCore;
using GradTest.Domain.Common.Entities;

namespace GradTest.Infrastructure.Common.Extensions;

public static class DbSetExtensions
{
    public static IQueryable<TSource> ExcludeSoftDeletedEntities<TSource>(this DbSet<TSource> dbSet)
        where TSource: EntityBase
    {
        return dbSet.Where(x => !x.IsDeleted);
    }
}