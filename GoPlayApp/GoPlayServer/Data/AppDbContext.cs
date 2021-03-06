using GoPlayServer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GoPlayServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions options ) : base(options) {}

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<ReportedPost> ReportedPosts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
