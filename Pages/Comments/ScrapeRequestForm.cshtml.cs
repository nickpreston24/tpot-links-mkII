using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;


namespace TPOT_Links.Pages.Comments;

public class ScrapeRequestFormModel : PageModel
{
    private static string airtable_api_key;
    public string AirtableAPIKey => airtable_api_key;
    public ScrapeRequestFormModel() 
    {
    }

    public void OnGet()
    {
        airtable_api_key = Environment.GetEnvironmentVariable("AIRTABLE_API_KEY");
        Console.WriteLine("api >> " + airtable_api_key);
    }

    public async Task<IActionResult> OnGetLogin(
        string email = "nick"
        , string password = "blah123") 
    {
        Console.WriteLine(email);
        Console.WriteLine(password);

        Console.WriteLine("returning fire!");
        return Partial("_ScrapeForm", "https://www.facebook.com/stephanie.reinke.92/posts/pfbid02Q9rYwrWFvme7e7VRXU1waKewW8VzUDAPCgi6k7q83yFzDsYvYYDpNKfHnn2EBHXl");
    }

}

