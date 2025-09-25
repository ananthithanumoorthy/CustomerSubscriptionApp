using CustomerSubscriptionApp.Web.Models;

namespace CustomerSubscriptionApp.Web.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<CustomerSubscription>> GetSubscriptionsAsync(string customerId, string? subscriptionName, DateTime? start, DateTime? end);
        Task<CustomerSubscription?> GetByIdAsync(int id);
        Task<CustomerSubscription> AddAsync(CustomerSubscription model);
        Task<CustomerSubscription?> UpdateAsync(CustomerSubscription model);
        Task<bool> DeleteAsync(int id);
        Task<CustomerSubscription?> UpdateCountAsync(string customerId, string subscriptionName, int delta);
    }
}
