namespace GradTest.Domain.BoundedContexts.Products.Enums;

using Ardalis.SmartEnum;

public sealed class Category : SmartEnum<Category>
{
    public static readonly Category Games = new Category(nameof(Games), 1);
    public static readonly Category Books = new Category(nameof(Books), 2);
    public static readonly Category Figures = new Category(nameof(Figures), 3);
    public static readonly Category Clothing = new Category(nameof(Clothing), 4);

    private Category(string name, int value) : base(name, value) { }
}
