using GoPlayServer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GoPlayServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions options ) : base(options) {}

        public DbSet<RegularUser> RegularUsers { get; set; }
        public DbSet<SportsCenter> SportsCenters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
