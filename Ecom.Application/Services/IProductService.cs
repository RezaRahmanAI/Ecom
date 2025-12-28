using Ecom.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<int> CreateProductAsync( string name, decimal price, int stock );
    Task<bool> UpdateProductAsync(int id, string name, decimal price, int stock, bool isActive);
    Task<bool> DeleteProductAsync(int id);
}
