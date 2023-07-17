using System.Net;
using System.Net.Mail;
using System.Text;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;

namespace TPOT_Links.Pages.Admin.Emails;

public class MassEmailer
{
    private static StringBuilder error_messages = new StringBuilder();
    public string Errors => error_messages.ToString();

    public MassEmailer Send(MassEmail email)
    {
        try
        {
            using SmtpClient mail_client = new SmtpClient();
            using MailMessage mail_message =
                email.Map(_email =>
                {
                    var (subject, body, recipients, _) = _email;
                    var mailMessage = new MailMessage()
                    {
                    };
                    return mailMessage;
                });

            string gmail_passphrase = Environment.GetEnvironmentVariable("GMAIL_PASSPHRASE");

            mail_client.Host = email.Host;
            mail_client.Port = 587;
            mail_client.EnableSsl = true;
            mail_client.DeliveryMethod = SmtpDeliveryMethod.Network;
            mail_client.Credentials = new NetworkCredential(
                MassEmail.MyEmail
                , gmail_passphrase);

            mail_client.Send(mail_message);
        }
        catch (Exception ex)
        {
            ex.Message.Dump("Uh oh! Something went wrong!");
            error_messages.AppendLine(ex.Message);
            throw;
        }

        return this;
    }
}