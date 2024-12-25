using MailKit.Net.Smtp;
using MimeKit;


namespace UserAuthenticationAPI.Services.AuthenticationService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string emailTo, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var email = new MimeMessage();  // contains details like sender, recipient, subject, and body
            email.From.Add(new MailboxAddress("humos", smtpSettings["UserName"]));
            email.To.Add(new MailboxAddress("", emailTo));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder  // construct the email body
            {
                HtmlBody = message
            };
            email.Body = bodyBuilder.ToMessageBody();  // converts the BodyBuilder into the required MimeEntity format for the email body

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(smtpSettings["Host"], int.Parse(smtpSettings["Port"]), MailKit.Security.SecureSocketOptions.StartTls);  // Ensures the connection is encrypted using STARTTLS
                await client.AuthenticateAsync(smtpSettings["UserName"], smtpSettings["Password"]);
                await client.SendAsync(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
