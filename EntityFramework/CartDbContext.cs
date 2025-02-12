using Cart.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cart.Api.EntityFramework;

public class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }
    
    public DbSet<CartItem> CartItems { get; set; }
}