using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Domain.Models;
namespace ProductService.Application.Contracts.RepositoryContracts
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProducts(string UserId);
        public Task<Product> CreateProduct(string UserId, string productName, string description, int price);
        public Task<string> DeleteProduct(string UserId, string ProductId);

        public Task<string> ChangeProduct(string UserId, string ProductId, string productName, string description, int price);
    }
}
