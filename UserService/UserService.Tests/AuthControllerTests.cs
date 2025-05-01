using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.Application.Records;
using UserService.Application.Services;
using UserService.Domain.Models;
using UserService.Infrastructure.Databases;
using UserService.Web.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UserService.Tests
{
    public class AuthControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AuthController _controller;
        private readonly AuthService _authService;

        public AuthControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _authService = new AuthService(_context);

            _controller = new AuthController(_context)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

       

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenCredentialsInvalid()
        {
            
            var request = new Register("testuser", "test@example.com", "password123123123");

            var result = await _controller.Login(request);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.DoesNotContain(_controller.HttpContext.Response.Headers,
                h => h.Key == "Set-Cookie");
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenNewUser()
        {
            var request = new Register("testuser", "test@example.com", "password123");
            var result = await _controller.Register(request);

            Assert.IsType<OkResult>(result);
            Assert.Single(_context.Users.ToList());
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserExists()
        {
           
            var existingUser = User.Create(Guid.NewGuid(), "testuser", "test@example.com", BCrypt.Net.BCrypt.HashPassword("password123"), DateTime.Now);
            await _context.Users.AddAsync(existingUser);
            await _context.SaveChangesAsync();

            var request = new Register("testuser", "test@example.com", "password123");

            var result = await _controller.Register(request);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Single(_context.Users.ToList()); // Проверяем, что новый пользователь не добавился
        }

        [Fact]
        public async Task Register_ShouldReturn()
        {
            var request = new Register("testuser", "test@example.com", "password123");

            await _controller.Register(request);
            var user = await _context.Users.FirstOrDefaultAsync();

           
            Assert.NotNull(user);
            
        }
    }
}