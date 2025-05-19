using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Potato.DbContext.Models.Entity;

namespace Potato.DbContext
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Message> Messages { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();

        }

           protected override void OnModelCreating(ModelBuilder builder)
           {
               Console.WriteLine("OnModelCreating вызван");
               base.OnModelCreating(builder);
               builder.ApplyConfiguration<Friend>(new FriendConfiguration());
        }

    }
}
