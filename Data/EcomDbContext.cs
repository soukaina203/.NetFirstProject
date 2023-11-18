
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
namespace Context
{
public class EcomDbContext : DbContext
{
    public DbSet<Product> Products {get; set;}
    public DbSet<User> Users {get; set;}

    public DbSet<Discussion> Discussions {get; set;}
    public DbSet<Message> Messages {get; set;}
    
    public EcomDbContext(DbContextOptions<EcomDbContext> options):base(options)
    {
        
    }

  

}

}