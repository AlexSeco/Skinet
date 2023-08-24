using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Data;

public class Context : DbContext
{
    public DbSet<Products> Products { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
}
