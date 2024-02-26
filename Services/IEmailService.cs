namespace CodeMechanic.TrashStack;

public interface IEmailService
{
    Task SendAsync(string email, string name, string subject, string body);
}