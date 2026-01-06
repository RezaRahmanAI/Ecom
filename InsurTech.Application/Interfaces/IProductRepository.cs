using InsurTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurTech.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProducts();
    Task<Product?> GetById(int id);
    Task<int> AddProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}
