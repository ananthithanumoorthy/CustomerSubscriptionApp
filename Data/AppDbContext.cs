using Microsoft.EntityFrameworkCore;
using CustomerSubscriptionApp.Web.Models;
namespace CustomerSubscriptionApp.Web.Data
{
    
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

            public DbSet<UserMaster> UserMasters { get; set; }
            public DbSet<CustomerSubscription> CustomerSubscriptions { get; set; }

            protected override void OnModelCreating(ModelBuilder mb)
            {
                base.OnModelCreating(mb);
                mb.Entity<UserMaster>().HasIndex(u => u.UserName).IsUnique();
                mb.Entity<CustomerSubscription>().HasIndex(c => new { c.CustomerId, c.SubscriptionName });
            }
        }
    }

