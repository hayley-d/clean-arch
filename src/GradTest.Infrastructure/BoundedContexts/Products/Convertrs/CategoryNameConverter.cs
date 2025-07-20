using GradTest.Domain.BoundedContexts.Products.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GradTest.Infrastructure.BoundedContexts.Products.ValueConvertrs;

public class CategoryNameConverter : ValueConverter<Category, string>
{
    public CategoryNameConverter() : base(
        category => category.Name,
        name => Category.FromName(name, true)
    )
    { }
}