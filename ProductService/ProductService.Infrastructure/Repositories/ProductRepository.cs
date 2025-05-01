using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Contracts.RepositoryContracts;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        AppDbContext db;
        public ProductRepository(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<List<Product>> GetAllProducts(string UserId)
        {
           return await db.Products.ToListAsync();
        }
        public async Task<Product> GetProduct(string UserId,string ProductId)
        {
            return await db.Products.Where(p => p.UserId.ToLower()== UserId.ToLower()).FirstAsync(p1 => p1.Id == Guid.Parse(ProductId.ToLower()));
        }

        public async Task<Product> CreateProduct(string UserId,string productName,string description,int price)
        {
            Product product = Product.Create(Guid.NewGuid(),productName,description,price,UserId,DateTime.Now);

            await db.AddAsync(product);
            await db.SaveChangesAsync();
            return product; 
        }

        public async Task<string> DeleteProduct(string UserId,string ProductId)
        {
            Product product = await GetProduct(UserId,ProductId);
            if(product == null) return string.Empty;
            else
            {
                db.Remove(product);
                await db.SaveChangesAsync();
                return product.Name;
            }
        }
        public async Task<string> ChangeProduct(string UserId, string ProductId, string productName, string description, int price)
        {
            Product product = await GetProduct(UserId, ProductId);
            if (product == null) return string.Empty;
            else
            {
                db.Remove(product);
                await db.SaveChangesAsync();
                Product NewProduct = Product.Create(product.Id,productName,description,price,UserId,product.CreatedAt);
                await db.AddAsync(NewProduct);
                await db.SaveChangesAsync();
                return productName;
            }
        }
    }
}
