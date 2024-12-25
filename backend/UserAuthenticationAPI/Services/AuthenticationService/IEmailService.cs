namespace UserAuthenticationAPI.Services.AuthenticationService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailTo, string subject, string message);
    }
}
