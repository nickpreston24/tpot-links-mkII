using System.Net.Mail;

namespace CodeMechanic.TrashStack;

public class DemoEmailService : IEmailService
{
    public async Task SendAsync(string email, string name, string subject, string body)
    {
        using (var smtp = new SmtpClient())
        {
            smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            string pickupdir = @"/home/nick/Downloads/maildump/";
            if (!Directory.Exists(pickupdir))
                Directory.CreateDirectory(pickupdir);
            
            smtp.PickupDirectoryLocation = pickupdir;
            var message = new MailMessage
            {
                Body = body,
                Subject = subject,
                From = new MailAddress(email, name),
                IsBodyHtml = true
            };
            message.To.Add("contact@domain.com");
            await smtp.SendMailAsync(message);
        }
    }
}