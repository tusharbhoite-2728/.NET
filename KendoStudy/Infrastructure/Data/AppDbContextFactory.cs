using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Infrastructure.Data;

namespace Infrastructure.Data
{
    public class AppDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseMySql(
                "Server=localhost;Database=masterdb;User=Tushar;Password=Tushar@0288;",
                ServerVersion.AutoDetect(
                    "Server=localhost;Database=masterdb;User=Tushar;Password=Tushar@0288;"
                )
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }

}
