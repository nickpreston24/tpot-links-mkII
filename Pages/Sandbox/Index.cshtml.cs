
using System.Diagnostics;
using System.Text;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using TPOT_Links.Models;

namespace TPOT_Links.Pages.Sandbox;
//Note: to remove all comments, replace this with nothing:  // .*$
public class IndexModel : HighSpeedPageModel
{
    private static string _query { get;set; } = string.Empty;
    public string Query => _query;

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver) 
    : base(embeddedResourceQuery, driver)
    {
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetSearchByRegex(string term = "God")
    {
        string query = await embeddedResourceQuery
            .GetQueryAsync<IndexModel>(new StackTrace());

        // Make the code snippet purty :)
        // var query_lines = query.Split("\n").Dump("query lines");
        // _query = new StringBuilder(query)
        //     .AppendEach(
        //     query_lines
        //     ,line => $"""
        //         <pre class='text-sm' data-prefix="$"><code>{line}</code></pre>
        //     """)
        //     .ToString()
        // ;

        var search_parameters = new
        {
            regex = $"(?i).*{term}.*".Dump("regex")
        };

        var pages = await SearchNeo4J<Page>(query, search_parameters);

        // var papers = pages.Select(page=> new TPOTPaper()
        //     .With(paper=> {
        //         // Doing some ad-hoc mapping here b/c Page.cs maps from Neo4j and TPOTPaper.cs from MySql, to be resolved soon.
        //     })
        // ); 
        // pages.Dump("SEARCH RESULTS");
        // return Partial("_PageTable", results);
        string html = new StringBuilder()
            .AppendEach(
                pages, paper => 
        $"""
            <tr>
                <th class='text-primary'>{paper.Id}</th>
                <th class='text-accent'>{paper.Title}</th>
                <td class='text-secondary'>{paper.Status}</td>
                <td class='text-secondary'>{paper.Author}</td>
                <td class='text-accent'>{paper.Categories}</td>
                <td class='text-secondary'>{paper.Excerpt}</td>
            </tr>
        """).ToString();

        return Content(html);
    }
    
    public async Task<IActionResult> OnGetRecommendations()
    {
        var failure = Content(
        $"""
            <div class='alert alert-error'>
                <p class='text-3xl text-warning text-sh'>
                    An Error Occurred...  But fret not! Our team of intelligent lab mice are on the job!
                </p>
            </div>
        """);

        string query = "..."; // This can be ANY SQL query.  In my case, I'm using cypher, because it's lovely.

        // Magically infers from the tract that the current method name is referring to 'Recommendations.cypher'
        query = await embeddedResourceQuery
            .GetQueryAsync<IndexModel>(new StackTrace());

        if(string.IsNullOrEmpty(query))
            return failure;  // If for some reason, nothing comes back, alert the user with this div.

        // This can also be a template, if we want, but here's a fancy-schmancy use of the triple-double quotes to easily send back anything in C# directly to HTML/X:
        return Content(
        $"""
            <div class='alert alert-primary'>
                <p class='text-xl text-secondary text-sh'>
                {query}
                </p>
            </div>
        """);
    }

}


