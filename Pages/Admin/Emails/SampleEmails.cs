using System.Net.Mail;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;

namespace TPOT_Links.Pages.Admin.Emails;

public class SampleEmails
{
    public SampleEmails(
        string firstName
        , string body
        , params string[] recipients) =>
        (Subject, Body, Recipients) = (firstName, body, recipients.ToList());

    public void Deconstruct(
        out string subject
        , out string body
        , out string[] recipients
        , out List<Attachment> attachments
    )
    {
        subject = Subject;
        body = Body;
        recipients = Recipients.ToArray();
        attachments = Attachments;
    }


    public SampleEmails Prepare(string hint = ".*Resume.*")
    {
        var grepper = new Grepper()
        {
            RootPath = @"~/Downloads",
            FileSearchMask = "*.pdf",
            FileNamePattern = hint
        };

        var all_files_found = grepper.GetFileNames();

        all_files_found.Dump();

        foreach (var filepath in all_files_found)
        {
            var attachment = new Attachment(filepath); // here you can attach a file as a mail attachment  
            Attachments.Add(attachment);
        }

        return this;
    }

    public string Subject { get; set; } = "HELLO There!";
    public string Body { get; set; } = "THIs IS A TEST from Nick Preston";
    public string Host { get; set; } = "smtp.gmail.com";
    public const string MyEmail = "michael.n.preston@gmail.com";
    public List<string> Recipients { get; set; } = new List<string>() { MyEmail };

    public MailAddress From { get; set; } = new MailAddress(MyEmail, string.Empty);
    public List<Attachment> Attachments { get; set; } = new List<Attachment>();
}