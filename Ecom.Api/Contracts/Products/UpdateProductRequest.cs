namespace Ecom.Api.Contracts.Products;

public record UpdateProductRequest(string Name, decimal Price, int Stock, bool IsActive);
