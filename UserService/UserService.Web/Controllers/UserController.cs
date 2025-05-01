using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using UserService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Services;
using UserService.Application.Records;
using Microsoft.AspNetCore.Identity.Data;
using UserService.Infrastructure.Databases;
using UserService.Infrastructure.Repositories;
using Org.BouncyCastle.Asn1.Ocsp;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserRepository UserRep;
        AuthService auth;
        
        public UserController(AppDbContext db)
        {
            UserRep = new UserRepository(db);
            auth = new AuthService(db);
            //this.db = db;
        }
        //UserManager user = new UserManager();
        
        [HttpGet("GetAllUsers")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            
            return Ok(await UserRep.GetAllUsers());
        }
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(string userId)
        {
            User user = await UserRep.GetUserById(userId);
            if (user == null) return BadRequest("No user with provided Id");
            return Ok(user);
        }

       
        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> Delete(string userId)
        {
            User user = await UserRep.DeleteUserById(userId);
            if (user == null) return BadRequest("No user with provided Id");
            return Ok("User was deleted");
        }
        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> ChangeUserById([FromBody] ChangeUser userChange)
        {
            User user = await UserRep.DeleteUserById(userChange.id);
            if (user == null) return BadRequest("No user with provided email");
            var answer = await auth.Register(userChange.UserName, userChange.email, userChange.Password);
            return Ok("User successfully changed");
        }
    }
}
