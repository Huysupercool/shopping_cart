using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace shopping_cart.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
		public DbSet<Product> Products { get; set; } = null;
		public DbSet<Order> Orders { get; set; } = null;
		public DbSet<Category> Categories { get; set; } = null;
		public DbSet<OrderProduct> OrderProducts { get; set; } = null;
	}
}
