using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TPOT_Links.Pages.Admin.Emails;

public class Index : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostBulkSendEmails(MassEmailer massEmailers)
    {
        return Content("<p class='alert alert-success'>Sent!</p>");
    }
}