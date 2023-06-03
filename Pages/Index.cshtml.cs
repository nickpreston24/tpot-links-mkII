using System.Diagnostics;
using System.Text;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;
using Newtonsoft.Json;

namespace TPOT_Links.Pages;

public class IndexModel : HighSpeedPageModel
{
    private readonly ILogger<IndexModel> _logger;
    private static List<PageRoute> my_routes = new List<PageRoute>();
    public List<PageRoute> MyRoutes => my_routes;

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver = null
        , IAirtableRepo repo = null)
        : base(embeddedResourceQuery, driver, repo)
    {
    }

    public async Task OnGet()
    {
        //// Reads from file system...
        // await using Stream stream = embeddedResourceQuery.Read<IndexModel>("../../routes.json");
        // string query = await stream.ReadAllLinesFromStreamAsync();
        // Console.WriteLine(query);
        // Debug.WriteLine(query);

        string routes_json = """
            [{"url":"/Admin/FeatureRequest","text":"FeatureRequest","enabled":true,"external":false},{"url":"/Admin/Index","text":"Index","enabled":true,"external":false},{"url":"/Comments/FinishedScrapes","text":"FinishedScrapes","enabled":true,"external":false},{"url":"/Comments/Index","text":"Index","enabled":true,"external":false},{"url":"/Comments/Manual_Copies","text":"Manual_Copies","enabled":true,"external":false},{"url":"/Comments/ScrapeRequestForm","text":"ScrapeRequestForm","enabled":true,"external":false},{"url":"/Comments/Scrapes","text":"Scrapes","enabled":true,"external":false},{"url":"/Comments/ScriptureHighlighter","text":"ScriptureHighlighter","enabled":true,"external":false},{"url":"/Error","text":"Error","enabled":true,"external":false},{"url":"/Index","text":"Index","enabled":true,"external":false},{"url":"/Logs/Index","text":"Index","enabled":true,"external":false},{"url":"/Markdown/Index","text":"Index","enabled":true,"external":false},{"url":"/Privacy","text":"Privacy","enabled":true,"external":false},{"url":"/Sandbox/.old/Index","text":"Index","enabled":true,"external":false},{"url":"/Sandbox/Index","text":"Index","enabled":true,"external":false},{"url":"/Tutorial/HxPatchExample","text":"HxPatchExample","enabled":true,"external":false},{"url":"/Tutorial/HxPostExample","text":"HxPostExample","enabled":true,"external":false},{"url":"/Tutorial/Index","text":"Index","enabled":true,"external":false},{"url":"/Uploads/Index","text":"Index","enabled":true,"external":false}]
        """;

        var routes = JsonConvert.DeserializeObject<List<PageRoute>>(routes_json);
        string pattern = """(?<parent>.*)\/(?<pagename>\w+)""";

        var all_routes = routes
                .SelectMany(route => route.url
                    .Extract<BreadCrumbParts>(pattern))
            ;

        // all_routes.Dump("all_routes");

        var routes_w_breadcrumbs = all_routes
            .Select(r => new PageRoute(r))
            // .Dump("routes w/ bread")
            ;

        my_routes = routes_w_breadcrumbs.ToList();
    }

    public IActionResult OnGetMyRoutes()
    {
        string html = new StringBuilder()
            .AppendEach(my_routes, route =>
                $"""
                <div class='card'>
                    <div class='card-body flex flex-col'>

                        <h1 class='text-sm'>{route.text}</h1>
                        <li class='link'>{route?.url ?? "(none)"}</li>
                    </div>
                </div>
            """)
            .ToString()
            .Tag("ul", className: "text-sm flex flex-col gap-4");
        
        return Content(html);
    }
}

public class BreadCrumbParts
{
    public string Breadcrumb { get; set; } = string.Empty;
    public string Parent { get; set; } = string.Empty;
    public string PageName { get; set; } = string.Empty;
}

public class PageRoute
{
    public string url { get; set; } = string.Empty;
    public string parent { get; set; } = string.Empty;
    public string Breadcrumb { get; set; } = string.Empty;
    public string text { get; set; } = string.Empty;
    public bool enabled { get; set; }
    public bool external { get; set; }

    public PageRoute(BreadCrumbParts parts)
    {
        string value = parts
            .ToMaybe()
            .IfSome(crumb => !string.IsNullOrWhiteSpace(crumb.PageName)
                ? crumb.PageName
                : "???");

        text = value;
        parent = parts?.Parent;
    }
}