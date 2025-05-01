using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Records;
using UserService.Domain.Models;
using UserService.Infrastructure.Databases;
using UserService.Infrastructure.Repositories;
using UserService.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using UserService.Application.Services;

namespace UserService.Tests
{
    public class UserControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UserController _controller;
        private readonly UserRepository _userRepo;
        private readonly AuthService _authService;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _userRepo = new UserRepository(_context);
            _authService = new AuthService(_context);

            _controller = new UserController(_context)
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
        public async Task GetAll_ShouldReturnAllUsers()
        {
        
            var users = new List<User>
            {
                User.Create(Guid.NewGuid(), "testuser1", "test1@example.com", BCrypt.Net.BCrypt.HashPassword("password1234"), DateTime.Now),
                User.Create(Guid.NewGuid(), "testuser2", "test2@example.com", BCrypt.Net.BCrypt.HashPassword("password123213213213213"), DateTime.Now)
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }


        [Fact]
        public async Task GetUser_ShouldReturnBadRequest_WhenNotExists()
        {
            
            var result = await _controller.GetUser(Guid.NewGuid().ToString());

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenNotExists()
        {
            
            var result = await _controller.Delete(Guid.NewGuid().ToString());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        
        
    }
}