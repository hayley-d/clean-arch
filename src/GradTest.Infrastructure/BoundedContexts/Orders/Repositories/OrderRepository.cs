using GradTest.Infrastructure.Common.Repository;
using GradTest.Infrastructure.Persistence;

namespace GradTest.Infrastructure.BoundedContexts.Ordrs.Repositories;

public class OrderRepository: BaseRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context) { }
}