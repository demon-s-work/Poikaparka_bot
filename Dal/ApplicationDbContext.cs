using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Poikaparka.Dal
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<User> Users { get; set; }
	}
}