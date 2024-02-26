using CodeMechanic.TrashStack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TPOT_Links.Pages;

public class ContactModel : PageModel
{
    private readonly IRenderingService _renderer;
    private readonly IEmailService _emailer;

    public ContactModel(IRenderingService renderer, IEmailService emailer)
    {
        _renderer = renderer;
        _emailer = emailer;
    }

    [BindProperty] public ContactForm ContactForm { get; set; }
    [TempData] public string PostResult { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var body = await _renderer.RenderAsync("_ContactEmailPartial", ContactForm);
        await _emailer.SendAsync(ContactForm.Email, ContactForm.Name, ContactForm.Subject, body);
        PostResult = "Check your specified pickup directory";
        return RedirectToPage();
    }
}

public class ContactForm
{
    public string Email { get; set; }
    public string Message { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public Priority Priority { get; set; }
}

public enum Priority
{
    Low,
    Medium,
    High
}