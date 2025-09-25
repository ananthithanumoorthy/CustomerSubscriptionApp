namespace CustomerSubscriptionApp.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class ConsoleEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine("=== Dev Email ===");
            Console.WriteLine($"To: {to}\nSubject: {subject}\n{body}");
            Console.WriteLine("=================");
            return Task.CompletedTask;
        }
    }
}
