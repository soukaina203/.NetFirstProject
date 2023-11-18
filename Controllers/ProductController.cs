using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Context;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Controllers;
[ApiController]
[Route("api/[controller]/{action}")]
public class ProductController : ControllerBase
{
    private readonly EcomDbContext _context;

    public ProductController(EcomDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get()
    {
        return await _context.Products.ToListAsync();
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<string>> Post([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return "done";
    }


    // PUT: api/products/{id}
    [HttpPatch("{id}")]
    public async Task<ActionResult<string>> Update([FromForm] Product product)
    {


        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return "Done";
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var productToDelete = await _context.Products.FindAsync(id);
        if (productToDelete == null)
        {
            return NotFound();
        }

        _context.Products.Remove(productToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }
};
