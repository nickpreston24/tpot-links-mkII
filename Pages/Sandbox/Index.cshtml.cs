using System.Diagnostics;
using System.Text;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using TPOT_Links.Models;
using TypeExtensions = CodeMechanic.Extensions.TypeExtensions;

namespace TPOT_Links.Pages.Sandbox;

//Note: to remove all comments, replace this with nothing:  // .*$
[BindProperties]
public class IndexModel : HighSpeedPageModel
{
    // private static string _query { get; set; } = string.Empty;
    // public string Query => _query;
    //
    //
    public string category { get; set; } = string.Empty;
    public bool search_by_categories { get; set; }

    public int CategoryNumber { get; set; } = -1;

    public bool IsSelected(string name) =>
        name.Equals(CurrentPanel?.Trim(), StringComparison.OrdinalIgnoreCase);

    public string CurrentPanel { get; set; } = string.Empty;
    public CardOptions CardOptions { get; set; } = new CardOptions();

    public Paper SelectedPaper { get; set; } = new Paper();
    public User CurrentUser { get; set; } = new User();

    // public List<Panel> Panels { get; set; } = new List<Panel>()
    // {
    //     new Panel
    //     {
    //         panel_name = "_PaperList",
    //         message = "Yarrr",
    //         title = "List of Papers"
    //     }
    // };


    public Stopwatch watch = new Stopwatch();

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver)
        : base(embeddedResourceQuery, driver)
    {
    }

    // public async Task<IActionResult> OnGetBloopf(string email)
    // {
    //     email.Dump("index");
    //     return Content("<p>boop!</p>");
    // }


    public async Task<IActionResult> OnPostBloopf()
    {
        // email.Dump("index");
        return Content("<p>boop!</p>");
    }

    
    
    public async Task<IActionResult> OnGetLikePaper(
        // [FromBody] Paper selected_paper
    )
    {
        var user = CurrentUser;
        user.Dump("dis user");
        string query = "";

        return Content("<p class='alert alert-success'>Liked!</p>");
    }
    
    //
    // public async Task<IActionResult> OnPostLikePaper(
    //     // [FromBody] Paper selected_paper
    // )
    // {
    //     var user = this.CurrentUser;
    //     user.Dump("dis user");
    //     string query = "";
    //
    //     return Content("<p class='alert alert-success'>Liked!</p>");
    // }

    public async Task<IActionResult> OnPostValidateUser([FromForm] User user, string pass = "")
    {
        // CurrentUser = TypeExtensions.With(u =>
        // {
        // });

        var pwd = Environment.GetEnvironmentVariable("TPOT_DEFAULT_PASSWORD");
        return Partial("_ValidatedUser", CurrentUser);
    }


    public async Task<IActionResult> OnGetSearchByRegex(
        string term = ""
        , bool show_excerpts = true
        , string show_slugs = ""
        , string show_urls = ""
        , string partial_name = "_PaperTable"
        , int limit = 20
    )
    {
        partial_name.Dump("yo");
        string query = await embeddedResourceQuery
            .GetQueryAsync<IndexModel>(new StackTrace());

        var category = search_by_categories ? CategoryNumber.ToString() : "";
        var search_parameters = new PaperSearch
                {
                    regex = $"""(?is)(<\w+>)?.*({term}).*(<\w+>)?""",
                    term = term,
                    category = category,
                    limit = limit
                }
                .Dump("paper search")
            ;

        watch.Start();

        var pages = await SearchNeo4J<Paper>(query, search_parameters);
        watch.Stop();

        return Partial(partial_name, pages);
        // return Partial("_PaperList", pages);
    }

    public async Task<IActionResult> OnPostBulkCreatePapers(
        string title = "")
    {
        // System.Console.WriteLine("creating batch of papers...");
        // Debug.WriteLine("creating batch of papers...".Dump());
        var batch_of_papers = new Paper
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
            var created = await BulkCreateNodes<Paper>(query, parameters);
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
        string query = "...";

        query = await embeddedResourceQuery
            .GetQueryAsync<IndexModel>(new StackTrace());

        if (string.IsNullOrEmpty(query))
            return
                Partial("_Alert");
        var search_parameters = new PaperSearch
                {
                    category = category,
                }
                .Dump()
            ;

        // search_parameters.Dump("s");
        var pages = await SearchNeo4J<Paper>(query, search_parameters);
        pages.FirstOrDefault().Dump("recommendations");

        return Partial("_PaperList", pages);
    }


    public string? IsSelectedClassName(string panel_name, string cssClass)
    {
        return IsSelected(panel_name) ? cssClass : null;
    }
}

public class CardOptions
{
    public bool show_slugs { get; set; }
    public bool show_excerpts { get; set; }
    public bool show_urls { get; set; }
    public bool case_insensitive { get; set; }
}


// pages.ToList().Count.Dump("# of recommendations");
//         string html = new StringBuilder()
//             .AppendEach(
//                 pages, paper =>
//                     $"""
//             <tr>
//                 <th class='text-primary'>{paper.Id}</th>
//                 <th class='text-accent'>{paper.Title}</th>
//                 <td class='text-secondary'>{paper.Status}</td>
//                 <td class='text-secondary'>{paper.Author}</td>
//                 <td class='text-accent'>{paper.Categories}</td>
//                 <td class='text-secondary'>{paper.Excerpt}</td>
//             </tr>
//         """).ToString();
//
//         return Content(html);