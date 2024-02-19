using CodeMechanic.Embeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Admin;
//Note: to remove all comments, replace this pattern with nothing:  // .*$

[BindProperties]
public class IndexModel : PageModel
{
    private readonly IEmbeddedResourceQuery embeddedResourceQuery;
    private readonly IDriver driver;
    private readonly IAirtableRepo airtable_repo;

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