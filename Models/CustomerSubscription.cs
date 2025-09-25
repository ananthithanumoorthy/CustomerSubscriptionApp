using System;
using System.ComponentModel.DataAnnotations;
namespace CustomerSubscriptionApp.Web.Models


{
    public class CustomerSubscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; } = null!;

        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        public string SubscriptionName { get; set; } = null!;

        public int SubscriptionCount { get; set; } = 0;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        public bool Active { get; set; } = true;
    }
}
