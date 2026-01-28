using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;


namespace Infrastructure.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options) { }
        public DbSet<Master> Master { get; set; }
    }
}
