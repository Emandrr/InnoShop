using Microsoft.AspNetCore.Mvc;
using UserService.Application.Records;
using UserService.Domain.Models;
using UserService.Application.Services;
using System.Threading.Tasks;
using UserService.Infrastructure.Databases;
namespace UserService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        AuthService auth;
        AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
            auth = new AuthService(context);
        }
        
        [HttpPost("login")]
         public async Task<IActionResult> Login([FromBody] Register request)
         {
            var user_info = await auth.Login(request.UserName, request.email, request.Password);
            if (user_info == null) return BadRequest("No user with provided data");
            else
            {
                HttpContext.Response.Cookies.Append("cookies", user_info[0]);
                HttpContext.Response.Cookies.Append("user_id_cookies", user_info[1]);
                
            }
           
           
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register request)
        {

            var answer = await auth.Register(request.UserName, request.email, request.Password);
            if (answer==null)
            {
                return BadRequest("User already exists.");
            }
            return Ok();
        }
    }
}
