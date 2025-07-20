namespace GradTest.Presentation;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public static class Products
    {
        private const string ProductBase = $"{ApiBase}/products";
        public const string GetProductById = $"{ProductBase}/{{id}}";
        public const string CreateProduct = ProductBase;
        public const string ListProducts = ProductBase;
    }
    
    public static class Orders
    {
        private const string OrderBase = $"{ApiBase}/orders";
        public const string GetOrderById = $"{OrderBase}/{{id}}";
        public const string CreateOrder = OrderBase;
    }
}