using CustomerSubscriptionApp.Web.Models;

namespace CustomerSubscriptionApp.Web.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<CustomerSubscription>> GetSubscriptionsAsync(string customerId, string? subscriptionName, DateTime? start, DateTime? end);

        // Get single subscription by customerId + subscriptionName
        Task<CustomerSubscription?> GetSubscriptionAsync(string customerId, string subscriptionName);

        // Add / Update / Delete by customerId + subscriptionName
        Task<CustomerSubscription> AddAsync(CustomerSubscription model);
        Task<CustomerSubscription?> UpdateAsync(string customerId, string subscriptionName, CustomerSubscription model);
        Task<bool> DeleteAsync(string customerId, string subscriptionName);

        // Update count
        Task<CustomerSubscription?> UpdateCountAsync(string customerId, string subscriptionName, int delta);
    }
}
