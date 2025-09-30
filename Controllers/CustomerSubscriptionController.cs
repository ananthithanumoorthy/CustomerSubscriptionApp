using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerSubscriptionApp.Web.Models;
using CustomerSubscriptionApp.Web.Services;

namespace CustomerSubscriptionApp.Web.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class CustomerSubscriptionController : Controller
    {
        private readonly ISubscriptionService _svc;
        public CustomerSubscriptionController(ISubscriptionService svc)
        {
            _svc = svc;
        }

        [HttpGet("Index")]
        public IActionResult Index() => View();

        // ---------------- Get subscriptions ----------------
        [HttpGet("/api/subscriptions")]
public async Task<IActionResult> Get(
    string? customerId,
    string? subscriptionName,
    string? startDate,
    string? endDate)
        {
            DateTime? start = null;
            DateTime? end = null;

            if (!string.IsNullOrWhiteSpace(startDate) && DateTime.TryParse(startDate, out var s))
                start = s;

            if (!string.IsNullOrWhiteSpace(endDate) && DateTime.TryParse(endDate, out var e))
                end = e;

            var data = await _svc.GetSubscriptionsAsync(customerId, subscriptionName, start, end);
            return Ok(data);
        }


        // ---------------- Add subscription ----------------
        [HttpPost("/api/subscriptions/add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CustomerSubscription model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Check if subscription already exists for the customer
            var existing = await _svc.GetSubscriptionAsync(model.CustomerId, model.SubscriptionName);
            if (existing != null) return BadRequest("Subscription already exists for this customer.");

            var created = await _svc.AddAsync(model);
            return Ok(created);
        }

        // ---------------- Update subscription ----------------
        [HttpPost("/api/subscriptions/update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] CustomerSubscription model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _svc.UpdateAsync(model.CustomerId, model.SubscriptionName, model);
            if (updated == null) return NotFound("Subscription not found for update.");

            return Ok(updated);
        }

        // ---------------- Delete subscription ----------------
        [HttpPost("/api/subscriptions/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody] CustomerSubscription model)
        {
            var ok = await _svc.DeleteAsync(model.CustomerId, model.SubscriptionName);
            if (!ok) return NotFound("Subscription not found for deletion.");
            return NoContent();
        }

        // ---------------- Update subscription count ----------------
        [HttpPost("/api/subscriptions/{customerId}/{subscriptionName}/count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCount(string customerId, string subscriptionName, [FromQuery] string action)
        {
            int delta = 0;
            if (string.Equals(action, "increment", StringComparison.OrdinalIgnoreCase)) delta = 1;
            else if (string.Equals(action, "decrement", StringComparison.OrdinalIgnoreCase)) delta = -1;
            else return BadRequest("action must be 'increment' or 'decrement'");

            var updated = await _svc.UpdateCountAsync(customerId, subscriptionName, delta);
            if (updated == null) return NotFound("Subscription not found for count update.");
            return Ok(updated);
        }
    }
}
