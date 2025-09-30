using CustomerSubscriptionApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerSubscriptionApp.Web.Data
{
    /// <summary>
    /// Runtime initializer to seed database with users & subscriptions
    /// </summary>
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Create DB if not exists
            await context.Database.EnsureCreatedAsync();

            // ---------------------------
            // Seed Users
            // ---------------------------
            if (!await context.UserMasters.AnyAsync())
            {
                var hasher = new PasswordHasher<UserMaster>();

                var users = new List<UserMaster>
                {
                    new UserMaster
                    {
                        UserName = "admin@local.test",
                        Email = "admin@local.test",
                        Role = "Admin",
                        Status = 1
                    },
                    new UserMaster
                    {
                        UserName = "manager@local.test",
                        Email = "manager@local.test",
                        Role = "Manager",
                        Status = 1
                    },
                    new UserMaster
                    {
                        UserName = "user@local.test",
                        Email = "user@local.test",
                        Role = "User",
                        Status = 1
                    },
                    new UserMaster
                    {
                        UserName = "readonly@local.test",
                        Email = "readonly@local.test",
                        Role = "Viewer",
                        Status = 1
                    }
                };

                // Hash passwords
                users[0].PasswordHash = hasher.HashPassword(users[0], "admin123");
                users[1].PasswordHash = hasher.HashPassword(users[1], "manager123");
                users[2].PasswordHash = hasher.HashPassword(users[2], "user123");
                users[3].PasswordHash = hasher.HashPassword(users[3], "readonly123");

                await context.UserMasters.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            // ---------------------------
            // Seed Subscriptions
            // ---------------------------
            if (!await context.CustomerSubscriptions.AnyAsync())
            {
                var subscriptions = new List<CustomerSubscription>
                {
                    new CustomerSubscription
                    {
                        CustomerId = "CUST001",
                        CustomerName = "Acme Corp",
                        SubscriptionName = "Basic Plan",
                        SubscriptionCount = 3,
                        StartDate = DateTime.UtcNow.Date.AddMonths(-1),
                        EndDate = DateTime.UtcNow.Date.AddMonths(11),
                        Active = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new CustomerSubscription
                    {
                        CustomerId = "CUST001",
                        CustomerName = "Acme Corp",
                        SubscriptionName = "Premium Addon",
                        SubscriptionCount = 1,
                        StartDate = DateTime.UtcNow.Date.AddDays(-10),
                        EndDate = DateTime.UtcNow.Date.AddMonths(6),
                        Active = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new CustomerSubscription
                    {
                        CustomerId = "CUST002",
                        CustomerName = "Beta Ltd",
                        SubscriptionName = "Standard Plan",
                        SubscriptionCount = 5,
                        StartDate = DateTime.UtcNow.Date.AddMonths(-2),
                        EndDate = DateTime.UtcNow.Date.AddMonths(10),
                        Active = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.CustomerSubscriptions.AddRangeAsync(subscriptions);
                await context.SaveChangesAsync();
            }
        }
    }
}
