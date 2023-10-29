using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
namespace Context
{
public class EcomDbContext : DbContext
{
    public DbSet<Product> Products {get; set;}
    public EcomDbContext(DbContextOptions<EcomDbContext> options):base(options)
    {
        
    }

  

}

}