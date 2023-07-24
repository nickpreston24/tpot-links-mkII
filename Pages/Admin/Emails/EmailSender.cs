using System.Net;
using System.Net.Mail;

namespace TPOT_Links.Pages.Admin.Emails;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.office365.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(SampleEmails.MyEmail,
                Environment.GetEnvironmentVariable("GMAIL_PASSPHRASE"))
        };

        return client.SendMailAsync(
            new MailMessage(from: "your.email@live.com",
                to: email,
                subject,
                message
            ));
    }
}

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}