using Pustok.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Pustok.Services.Implements;

public class EmailService : IEmailService
{
    readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Send(string userEmail, string Subject, string Body, bool IsBody = true)
    {
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Port = Convert.ToInt32(_configuration["Email:Port"]);
        smtpClient.Host = _configuration["Email:Host"];
        smtpClient.EnableSsl = true;

        NetworkCredential credential = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]);
        smtpClient.Credentials = credential;

        MailMessage message = new MailMessage();
        message.From = new MailAddress(_configuration["Email:Username"], "Pustok support");
        message.To.Add(new MailAddress(userEmail));
        message.Subject = Subject;
        message.Body = Body;
        message.IsBodyHtml = IsBody;

        smtpClient.Send(message);
    }

}
