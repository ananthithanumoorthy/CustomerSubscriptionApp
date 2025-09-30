using CustomerSubscriptionApp.Web.Data;
using CustomerSubscriptionApp.Web.Models;
using CustomerSubscriptionApp.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace CustomerSubscriptionApp.Web.Repositories
{
    public class SubscriptionRepository : ISubscriptionService
    {
        private readonly AppDbContext _db;
        public SubscriptionRepository(AppDbContext db) { _db = db; }

        public async Task<CustomerSubscription> AddAsync(CustomerSubscription model)
        {
            _db.CustomerSubscriptions.Add(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteAsync(string customerId, string subscriptionName)
        {
            var e = await _db.CustomerSubscriptions
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.SubscriptionName == subscriptionName);
            if (e == null) return false;
            _db.CustomerSubscriptions.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<CustomerSubscription?> GetSubscriptionAsync(string customerId, string subscriptionName)
        {
            return await _db.CustomerSubscriptions
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.SubscriptionName == subscriptionName);
        }

        public async Task<IEnumerable<CustomerSubscription>> GetSubscriptionsAsync(
     string? customerId,
     string? subscriptionName,
     DateTime? startDate,
     DateTime? endDate)
        {
            var query = _db.CustomerSubscriptions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerId))
                query = query.Where(s => s.CustomerId == customerId);

            if (!string.IsNullOrWhiteSpace(subscriptionName))
                query = query.Where(s => s.SubscriptionName.Contains(subscriptionName));

            if (startDate.HasValue)
                query = query.Where(s => s.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(s => s.EndDate <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task<CustomerSubscription?> UpdateAsync(string customerId, string subscriptionName, CustomerSubscription model)
        {
            var existing = await _db.CustomerSubscriptions
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.SubscriptionName == subscriptionName);
            if (existing == null) return null;

            existing.CustomerName = model.CustomerName;
            existing.SubscriptionName = model.SubscriptionName;
            existing.SubscriptionCount = model.SubscriptionCount;
            existing.StartDate = model.StartDate;
            existing.EndDate = model.EndDate;
            existing.Active = model.Active;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<CustomerSubscription?> UpdateCountAsync(string customerId, string subscriptionName, int delta)
        {
            var e = await _db.CustomerSubscriptions
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.SubscriptionName == subscriptionName);
            if (e == null) return null;

            e.SubscriptionCount += delta;
            if (e.SubscriptionCount < 0) e.SubscriptionCount = 0;

            await _db.SaveChangesAsync();
            return e;
        }
    }
}
