using CodeMechanic.Embeds;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages;

public class IndexModel : PageModel
{
    private readonly IEmbeddedResourceQuery embeddedResourceQuery;
    private readonly IDriver driver;
    private readonly IAirtableRepo airtable_repo;

    // private readonly ILogger<IndexModel> _logger;
    // private static List<RazorPageRoute> my_routes = new List<RazorPageRoute>();
    // public List<RazorPageRoute> MyRoutes => my_routes;

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver = null
        , IAirtableRepo airtableRepo = null)
    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        this.driver = driver;
        this.airtable_repo = airtableRepo;
    }
}