using Microsoft.EntityFrameworkCore;
using System;
namespace Auth.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
           : base(options) { }
    }
}
