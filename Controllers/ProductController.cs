using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductData;
using Models;
using Microsoft.AspNetCore.Mvc;
namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]/{action}")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Product>> Get()
        {
            return Ok(EcomDbContext.Products);
        }

        [HttpGet("id")]
        public ActionResult<Product> Get(int id)
        {
            var item = EcomDbContext.Products.FirstOrDefault(u => u.Id == id);
            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpPost]
        [Route(template: "AddProduct")]
        public ActionResult<Product> AddProduct(Product product)
        {
            EcomDbContext.Products.Add(product);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var item = EcomDbContext.Products.FirstOrDefault(u => u.Id == id);
            if (item == null) return NotFound();
            EcomDbContext.Products.Remove(item);
            return Ok();
        }
        [HttpPatch]
        public ActionResult Update( Product product){
           var targetedProduct=EcomDbContext.Products.FirstOrDefault(u=>u.Id==product.Id);
            if(targetedProduct==null) return NotFound();
             targetedProduct.Name=product.Name;
             targetedProduct.Price=product.Price;
             return Ok();


        }
    }
}