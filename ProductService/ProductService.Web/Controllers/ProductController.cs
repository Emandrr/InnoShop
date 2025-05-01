using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using ProductService.Infrastructure.Repositories;
using ProductService.Domain.Models;
using ProductService.Application.Records;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Databases;
using System.Collections.Generic;
namespace ProductService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductRepository ProdRep;
        public ProductController(AppDbContext context)
        {
            ProdRep = new ProductRepository(context);
        }
        [HttpPost("CreateProduct")]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] ProductBody productChange)
        {

            var id = HttpContext.Request.Cookies["user_id_cookies"];
            var answer = await ProdRep.CreateProduct(id, productChange.Name, productChange.Description, productChange.Price);
          
            return Ok(answer);
        }
        [HttpGet("GetAllProducts")]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            
            var id = HttpContext.Request.Cookies["user_id_cookies"];
            var lst = await ProdRep.GetAllProducts(id);
            if (lst == null) return BadRequest();
            return Ok(lst);
        }
        [HttpGet("{productId}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(string productId)
        {
            var id = HttpContext.Request.Cookies["user_id_cookies"];
            Product product = await ProdRep.GetProduct(id, productId);
            if (product == null) return BadRequest();
            return Ok(product);
        }


        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> Delete(string productId)
        {
            var id = HttpContext.Request.Cookies["user_id_cookies"];
            string answer = await ProdRep.DeleteProduct(id,productId);
            if (answer == string.Empty) return BadRequest();
            return Ok("Product was deleted");
        }
        [HttpPut("{productId}")]
        [Authorize]
        public async Task<IActionResult> ChangeProduct([FromBody] ProductBody productChange)
        {
            var id = HttpContext.Request.Cookies["user_id_cookies"];
            string answer = await ProdRep.ChangeProduct(id,productChange.id,productChange.Name,productChange.Description,productChange.Price);
            if (answer == string.Empty) return BadRequest("No product with provided id");
            return Ok("Product successfully changed");
        }
    }
}
