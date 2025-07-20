using GradTest.Domain.Common.Rules;

namespace GradTest.Domain.BoundedContexts.Products.Rules;

public class ProductMustBeUniqueRule : AbstractRule
{
    private string Name { get; }

    protected override bool Passed()
    {
        // TODO: Implement logic
        //var productIsUnique = !Name.Equals();
        var productIsUnique = true;
        return productIsUnique;
    }

    private ProductMustBeUniqueRule(string name)
    {
        Name = name;
        Title = "Product must be unique";  
        ErrorDetails = "A product with the same name already exists.";
    }

    public static ProductMustBeUniqueRule Create(string name)
    {
        return new ProductMustBeUniqueRule(name);
    }
}
