using Microsoft.EntityFrameworkCore;
using FlightMVC.Models;

namespace FlightMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasIndex(f => new { f.FlightNumber, f.AirlineId, f.DateOfTravel })
                .IsUnique();
        }
    }
}
