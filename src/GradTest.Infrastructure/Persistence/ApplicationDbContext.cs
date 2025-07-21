using Microsoft.EntityFrameworkCore;
using GradTest.Domain.BoundedContexts.Orders.Entities;
using GradTest.Domain.BoundedContexts.Products.Entities;
using GradTest.Domain.Common.Entities;
using GradTest.Infrastructure.BoundedContexts.Products.ValueConvertrs;
using GradTest.Infrastructure.Common.Errors;
using GradTest.Shared.Monads;

namespace GradTest.Infrastructure.Persistence;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    
    public DbSet<ExchangeRate> ExchangeRates { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Order>(order =>
        {
            order.HasKey(o => o.Id);

            order.HasMany(order => order.Items)
                .WithOne(item => item.Order)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<OrderItem>()
            .HasIndex(item => new { item.OrderId, item.ProductId })
            .IsUnique();

        modelBuilder.Entity<Product>()
            .Property(p => p.Category)
            .HasConversion(new CategoryNameConverter());


        modelBuilder.Entity<OrderItem>(item =>
        {
            item.HasKey(i => i.Id);

            item.HasOne(item => item.Product)
                .WithMany() 
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 
        });
    }

    public new async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await base.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(count);
        }
        catch (Exception e)
        {
            return Result<int>.Error(DatabaseError.Create(e.Message));
        }
    }
}