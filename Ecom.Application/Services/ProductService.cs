using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;


namespace Ecom.Application.Services;

public class ProductService : IProductService
{

    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public Task<int> CreateProductAsync(string name, decimal price, int stock)
    {
        //if(string.IsNullOrEmpty(name))
        //    throw new ArgumentException("Product name cannot be null or empty.");
        //if(price <= 0) throw new ArgumentException("Product price must be greater than zero.");
        //if(stock < 0) throw new ArgumentException("Product stock cannot be negative.");

        //var item = new Product { Name= name, Price = price, Stock = stock, IsActive = true };

        //return _repo.AddProduct(item);
        throw new ArgumentException("Test error: Invalid product input");

    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var p = await _repo.GetById(id);
        if (p is null) return false;

        await _repo.DeleteProduct(p);
        return true;

    }    

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var items = await _repo.GetAllProducts();
        return items.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock, p.IsActive)).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var item = await _repo.GetById(id);

        return item is null ? null : new ProductDto(item.Id, item.Name, item.Price, item.Stock, item.IsActive);
    }

    public async Task<bool> UpdateProductAsync(int id, string name, decimal price, int stock, bool isActive)
    {
        var p = await _repo.GetById(id);
        if (p is null) return false;

        p.Name = name;
        p.Price = price;
        p.Stock = stock;
        p.IsActive = isActive;

        await _repo.UpdateProduct(p);
        return true;
    }
}
