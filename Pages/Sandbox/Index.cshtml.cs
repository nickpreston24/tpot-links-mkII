using System.Diagnostics;
using System.Text;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using CodeMechanic.Types;
using CodeMechanic.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using TPOT_Links.Models;

namespace TPOT_Links.Pages.Sandbox;

//Note: to remove all comments, replace this with nothing:  // .*$
[BindProperties]
public class IndexModel : HighSpeedPageModel
{
    private static string _query { get; set; } = string.Empty;
    public string Query => _query;

    public bool show_slugs { get; set; }
    public bool show_excerpts { get; set; }
    public bool show_urls { get; set; }
    public bool case_insensitive { get; set; }
    public string category { get; set; } = string.Empty;
    public bool search_by_categories { get; set; }
    public int Number { get; set; } = -1;
    public string Title { get; set; } = string.Empty;

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver)
        : base(embeddedResourceQuery, driver)
    {
    }

    public async Task<IActionResult> OnGetSearchByRegex(
        string term = "God"
        , bool show_excerpts = true
        , string show_slugs = ""
        , string show_urls = ""
    )
    {
        string query = await embeddedResourceQuery
            .GetQueryAsync<IndexModel>(new StackTrace());

        var category = search_by_categories ? this.Number.ToString() : "";
        var search_parameters = new PaperSearch
        {
            // regex = $"""(?i)(<\w+>)?.*{term}.*(<\w+>)?""",
            regex = $"""(?i)(<\w+>)?.*({term}).*(<\w+>)?""",
            term = term,
            category = category
        }
        .Dump("paper search");

        var pages = await SearchNeo4J<Page>(query, search_parameters);

        string html = new StringBuilder()
            .AppendEach(
                pages, paper =>
                    $"""
            <tr>
                <th class='text-primary'>{paper.Id}</th>
                <th class='text-accent'>{paper.Title}</th>
                <td class='text-secondary'>{paper.Excerpt}</td>
                <td class='text-secondary'>{paper.Content}</td>
                <td class='text-secondary'>{paper.Status}</td>
                <td class='text-secondary'>{paper.Author}</td>
                <td class='text-accent'>{paper.Categories}</td>
            </tr>
        """).ToString();

        return Content(html);
    }

    public async Task<IActionResult> OnPostBulkCreatePapers(
        string title = "")
    {
        // System.Console.WriteLine("creating batch of papers...");
        // Debug.WriteLine("creating batch of papers...".Dump());
        var batch_of_papers = new Page
        {
            Title = "Test Paper 2",
            Content = "<p>test2</p>",
            Rendered_Content = "<p>test2</p>",
            Url = "tpotexample2.com"
        }.AsList();

        string query = """
            UNWIND $batch AS map
            CREATE (pages:Page)
            SET pages = map
        """;

        var parameters = new
        {
            batch = batch_of_papers
        };

        var alert = (string text, string alert)
            => $"""<p class='alert alert-{alert}' x-init='loading=false'>{text}!<p>""";
        try
        {
            var created = await BulkCreateNodes<Page>(query, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // throw;
            return Content(alert($"FAILED{e.Message}", "error"));
        }

        return Content(alert("hey", "success"));
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

        if (string.IsNullOrEmpty(query))
            return failure; // If for some reason, nothing comes back, alert the user with this div.

        var search_parameters = new
        {
        };
        // search_parameters.Dump("s");
        var pages = await SearchNeo4J<Page>(query, search_parameters);
        // pages.FirstOrDefault().Dump("recommendations");
        // pages.ToList().Count.Dump("# of recommendations");
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
}