
using Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Models;
using Providers;
using System.Security.Claims;
namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {

        private readonly EcomDbContext _context;
        private readonly TokenHandler _tokkenHandler;
        private readonly Crypto  _crypto;


        public AccountController(EcomDbContext context,Crypto crypto,TokenHandler tokkenHandler)
        {
            _context = context;
            _crypto=crypto;
            _tokkenHandler=tokkenHandler;

        }
        [HttpPost("/api/Account/Register")]
        public async Task<IActionResult> Register(User model)
        {
            var emailExiste = await _context.Users.Where(e => e.Email == model.Email).FirstOrDefaultAsync(e => e.Email == model.Email);

            if (emailExiste != null)
            {
                return Ok(new { code = -1, message = "Email deja exister" });
            }
            model.Password=_crypto.HashPassword(model.Password);
            await _context.Users.AddAsync(model);
            await _context.SaveChangesAsync();
            return Ok("Done");



        }



        [HttpPost("/api/Account/Login")]

        public async Task<ActionResult<string>> Login(User model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Ok(new { message = "Email | password required", code = -4 });
            }
            var User = await _context.Users.Where(x => x.Email == model.Email).AsNoTracking().FirstOrDefaultAsync();
            var newHash = _crypto.HashPassword(model.Password);
            if (User == null)
            {
                return Ok(new { message = "Email error ", code = -3 });
            }
            if (newHash!=User.Password)
            {
                return Ok(new { message = "Error Password", code = -1 });
            }
            var claims = new Claim[]
                           {
                        new(ClaimTypes.Name, User.Id.ToString()),
                        new(ClaimTypes.Email, User.Email)};
                           
            User.Password="";
            var token = _tokkenHandler.GenerateTokken(claims);
            return Ok(new { User, Token = token });
        }
    }


}