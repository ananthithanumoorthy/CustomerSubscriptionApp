using CustomerSubscriptionApp.Web.Data;
using CustomerSubscriptionApp.Web.Models;
using CustomerSubscriptionApp.Web.Services;
using Microsoft.EntityFrameworkCore;
namespace CustomerSubscriptionApp.Web.Repositories
{
    public class SubscriptionRepository:ISubscriptionService
    {
        private readonly AppDbContext _db;
        public SubscriptionRepository(AppDbContext db) { _db = db; }

        public async Task<CustomerSubscription> AddAsync(CustomerSubscription model)
        {
            _db.CustomerSubscriptions.Add(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _db.CustomerSubscriptions.FindAsync(id);
            if (e == null) return false;
            _db.CustomerSubscriptions.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<CustomerSubscription?> GetByIdAsync(int id)
            => await _db.CustomerSubscriptions.FindAsync(id);

        public async Task<IEnumerable<CustomerSubscription>> GetSubscriptionsAsync(string customerId, string? subscriptionName, DateTime? start, DateTime? end)
        {
            var q = _db.CustomerSubscriptions.AsQueryable();
            q = q.Where(x => x.CustomerId == customerId);
            if (!string.IsNullOrEmpty(subscriptionName)) q = q.Where(x => x.SubscriptionName.Contains(subscriptionName));
            if (start.HasValue) q = q.Where(x => x.StartDate >= start.Value);
            if (end.HasValue) q = q.Where(x => x.EndDate <= end.Value);
            return await q.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<CustomerSubscription?> UpdateAsync(CustomerSubscription model)
        {
            var existing = await _db.CustomerSubscriptions.FindAsync(model.Id);
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
