using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;

using Context;
using Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly EcomDbContext _context;
        public UserController(EcomDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}"),Authorize]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var User = await _context.Users.FindAsync(id);
            if (User == null)
            {
                return NotFound();
            }

            return User;
        }
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] User User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return "done";
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<string>> Update([FromForm] User User)
        {


            _context.Entry(User).State = EntityState.Modified;
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


    }
}