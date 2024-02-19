using CodeMechanic.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using CodeMechanic.Embeds;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;
using TPOT_Links.Models;

namespace TPOT_Links.Pages.Comments;

[BindProperties]
public class IndexModel : PageModel
{
    private readonly IEmbeddedResourceQuery embeddedResourceQuery;
    private readonly IDriver driver;
    private readonly IAirtableRepo airtable_repo;
    private static readonly RestClient restClient = new RestClient("https://jsonplaceholder.typicode.com/");
    private static string airtable_api_key;
    public string AirtableAPIKey => airtable_api_key;

    public List<Scripture> scriptures = new List<Scripture>();

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver = null
        , IAirtableRepo airtableRepo = null
    )
    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        this.driver = driver;
        this.airtable_repo = airtableRepo;
    }

    public void OnGet()
    {
        airtable_api_key = Environment.GetEnvironmentVariable("AIRTABLE_API_KEY");
        Console.WriteLine("api >> " + airtable_api_key);
    }

    public async Task<IActionResult> OnGetSearch(string keyword = "faith")
    {
        var url = $"https://tpot-api.vercel.app/api/pages?keyword={keyword}";

        var request = new RestRequest("todos");
        var comments = await restClient.GetAsync<List<Comment>>(request);

        // comments.Dump();

        return Content($"{comments.Count}".Tag("p"));
    }

    public async Task<IActionResult> OnGetExtractedScriptures()
    {
        var failure_message = Content(
            $"""
            <div class='alert alert-error'>
              <p class='text-xl text-white text-sh'>An Error Occurred... But fret not! Our team of intelligent lab mice are on the
                job!</p>
            </div>
            """ );

        string[] all_sample_text = new string[] { }; //sample_comments.Concat(sample_teachings).ToArray();

        // (List<Scripture> prefixed
        //         , List<Scripture> postfixed
        //         , List<Scripture> full
        //     ) = new ScriptureParser(all_sample_text);

        List<Scripture> prefixed = new();
        List<Scripture> full = new();

        var extracted_scriptures_list =
            prefixed.Dump("prefixed")
                // .Concat(postfixed.Dump("postfixed"))
                .Concat(full.Dump("full"))
                // joins N array of strings into a single array
                .Select(scripture => $"""
              <div class="card w-96 bg-base-100 shadow-xl">
                <div class="card-body">
                  <h2 class="card-title">{ scripture.Name}                </h2>
                  <p>{ scripture.Text}                </p>
                </div>
              </div>
        """ ) // each scripture is now templated as a daisyui card.
                .FlattenText();

        var regex_button = $"""
        <a href="https://regex101.com/r/HiM1uO/1">
          <button class="btn btn-primary">See the Regex</button>
        </a>
      """ ;

        // await Task.Delay(1500);

        string result = """
            <div x-init='loading=false'">
                <p>Done.</p>
            </div>
        """;

        return Content(result);
    }


    public async Task<IActionResult> OnGetLogin(
        string email = "nick"
        , string password = "blah123")
    {
        Console.WriteLine(email);
        Console.WriteLine(password);

        Console.WriteLine("returning fire!");
        return Partial("_ScrapeForm",
            "https://www.facebook.com/stephanie.reinke.92/posts/pfbid02Q9rYwrWFvme7e7VRXU1waKewW8VzUDAPCgi6k7q83yFzDsYvYYDpNKfHnn2EBHXl");
    }

    
}