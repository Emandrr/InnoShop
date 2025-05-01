using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Records;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Databases;
using ProductService.Infrastructure.Repositories;
using ProductService.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Tests
{
    public class ProductControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ProductController _controller;
        private readonly ProductRepository _productRepo;

        public ProductControllerTests()
        {
            // Настройка InMemory Database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _productRepo = new ProductRepository(_context);

            // Инициализация контроллера
            _controller = new ProductController(_context)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = { Cookies = new MockCookieCollection("user_id_cookies", "test_user_id") }
                    }
                }
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenProductDeleted()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var userId = "test_user_id";

            // Добавляем тестовый продукт
            _context.Products.Add(Product.Create(productId, "Test Product", "description",100,userId,DateTime.Now));
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(productId.ToString());

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Null(await _context.Products.FindAsync(productId));
        }
        [Fact]
        public async Task CreateProduct_ShouldReturnProduct_WhenSuccess()
        {
            // Arrange
            var productBody = new ProductBody(Guid.NewGuid().ToString(),"New Product","Description",100);

            // Act
            var result = await _controller.CreateProduct(productBody);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Product>(okResult.Value);

            Assert.Equal(productBody.Name, product.Name);
            Assert.Equal("test_user_id", product.UserId);
            Assert.Single(_context.Products.ToList());
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnUserProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                Product.Create(Guid.NewGuid(), "Product 1", "Desc 1", 100, "test_user_id", DateTime.Now),
                Product.Create(Guid.NewGuid(), "Product 2", "Desc 2", 200, "test_user_id", DateTime.Now),
                Product.Create(Guid.NewGuid(), "Other User Product", "Desc", 300, "test_user_id", DateTime.Now)
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);

            Assert.Equal(3, returnedProducts.Count);
            Assert.All(returnedProducts, p => Assert.Equal("test_user_id", p.UserId));
        }


        [Fact]
        public async Task GetProduct_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = Product.Create(productId, "Test Product", "Desc", 100, "test_user_id", DateTime.Now);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetProduct(productId.ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);

            Assert.Equal(product.Id, returnedProduct.Id);
            Assert.Equal(product.Name, returnedProduct.Name);
        }




        private class MockCookieCollection : IRequestCookieCollection
        {
            private readonly Dictionary<string, string> _cookies;

            public MockCookieCollection(string key, string value)
            {
                _cookies = new Dictionary<string, string> { { key, value } };
            }

            public string this[string key] => _cookies[key];
            public int Count => _cookies.Count;
            public ICollection<string> Keys => _cookies.Keys;
            public bool ContainsKey(string key) => _cookies.ContainsKey(key);
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _cookies.GetEnumerator();
            public bool TryGetValue(string key, out string value) => _cookies.TryGetValue(key, out value);
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}