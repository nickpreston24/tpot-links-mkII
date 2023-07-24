using CodeMechanic.Airtable;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Admin;
//Note: to remove all comments, replace this pattern with nothing:  // .*$

[BindProperties]
public class IndexModel : HighSpeedPageModel
{
    public IndexModel(IEmbeddedResourceQuery embeddedResourceQuery
        , IAirtableRepo airtable
        , INeo4JRepo neorepo
    )
        : base(embeddedResourceQuery, airtable, neorepo)
    {
    }
}