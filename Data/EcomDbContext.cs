using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
namespace ProductData
{
public class EcomDbContext : DbContext
{
    // public DbSet<Product> Products { get; set; }

    public static  List<Product> Products=new List<Product> {
        new Product {  Id=1, Name="Kay", Price=152},
        new Product {  Id=2, Name="anu", Price=152},
        new Product {  Id=3, Name="cdhn", Price=152},
    };
}

}