using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerSubscriptionApp.Web.Models;
using CustomerSubscriptionApp.Web.Services;


namespace CustomerSubscriptionApp.Web.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class CustomerSubscriptionsController : Controller
    {
        private readonly ISubscriptionService _svc;
        public CustomerSubscriptionsController(ISubscriptionService svc) 
        {
            _svc = svc;
        }

        [HttpGet("Index")]
        public IActionResult Index() => View();

        // API endpoints for UI / external use
        [HttpGet("/api/subscriptions")]
        public async Task<IActionResult> Get([FromQuery] string customerId, [FromQuery] string? subscriptionName, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return BadRequest("CustomerId required");
            var list = await _svc.GetSubscriptionsAsync(customerId, subscriptionName, startDate, endDate);
            return Ok(list);
        }

        [HttpPost("/api/subscriptions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CustomerSubscription model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _svc.AddAsync(model);
            return CreatedAtAction(nameof(Get), new { customerId = created.CustomerId }, created);
        }

        [HttpPut("/api/subscriptions/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerSubscription model)
        {
            if (id != model.Id) return BadRequest("Id mismatch");
            var updated = await _svc.UpdateAsync(model);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("/api/subscriptions/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _svc.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPost("/api/subscriptions/{customerId}/{subscriptionName}/count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCount(string customerId, string subscriptionName, [FromQuery] string action)
        {
            int delta = 0;
            if (string.Equals(action, "increment", StringComparison.OrdinalIgnoreCase)) delta = 1;
            else if (string.Equals(action, "decrement", StringComparison.OrdinalIgnoreCase)) delta = -1;
            else return BadRequest("action must be 'increment' or 'decrement'");
            var updated = await _svc.UpdateCountAsync(customerId, subscriptionName, delta);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
