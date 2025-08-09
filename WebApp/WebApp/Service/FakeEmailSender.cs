using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Service
{
    public class FakeEmailSender : IEmailSender<ApplicationUser>
    {
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            Console.WriteLine($"Confirmation Link: {confirmationLink}");
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            Console.WriteLine($"Reset Link: {resetLink}");
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            Console.WriteLine($"Reset Code: {resetCode}");
            return Task.CompletedTask;
        }
    }
}
