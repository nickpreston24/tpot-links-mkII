using System.Diagnostics;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;
using NSpecifications;
using TPOT_Links.Extensions;
using TPOT_Links.Models;
using CodeMechanic.Neo4j;
using CodeMechanic.Neo4j.Repos;

namespace TPOT_Links.Pages.Sandbox;

//Note: to remove all comments, replace this with nothing:  // .*$
[BindProperties]
public class IndexModel : PageModel
{
    private readonly IEmbeddedResourceQuery embeddedResourceQuery;
    private readonly EmbeddedResourceService embedService;
    private readonly IDriver driver;
    private readonly IAirtableRepo repo;

    // private static string _query { get; set; } = string.Empty;
    // public string Query => _query;

    public string category { get; set; } = string.Empty;
    public bool search_by_categories { get; set; }

    public int CategoryNumber { get; set; } = -1;

    public bool IsSelected(string name) =>
        name.Equals(CurrentPanel?.Trim(), StringComparison.OrdinalIgnoreCase);

    public string CurrentPanel { get; set; } = string.Empty;
    public CardOptions CardOptions { get; set; } = new CardOptions();

    public Paper SelectedPaper { get; set; } = new Paper();
    private static List<Paper> _current_recommendations = new List<Paper>();
    public List<Paper> RecommendedPapers => _current_recommendations;

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


    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IEmbeddedResourceQuery embedService
        , IDriver driver
        , IAirtableRepo repo)

    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        this.embedService = embedService.Dump("injected?") as EmbeddedResourceService;
        this.embedService.GetFileContents<IndexModel>("SearchByRegex.cypher");
        this.driver = driver;
        this.repo = repo;
    }

    public void OnGet()
    {
        // string query2 = (embedService as EmbeddedResourceService).GetFileContents<IndexModel>("SearchByRegex.cypher");


        // var customAlert = new CustomAlert()
        // {
        //     Message = $"test message",
        //     AlertType = "error"
        // }.Dump("custom alert");
    }

    public async Task<IActionResult> OnGetLikePaper(
        // [FromBody] Paper selected_paper
    )
    {
        var user = CurrentUser;
        user.Dump("dis user");
        // string query = "";

        return Content("<p x-on:init='show_modal=true' class='alert alert-success'>Liked!</p>");
    }

    public async Task<IActionResult> OnPostValidateUser([FromForm] User user, string pass = "")
    {
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
        try
        {
            var fn = (() => this.embedService.GetFileContents<IndexModel>("SearchByRegex.cypher"));
            string query = fn.QuickWatch("read speed");
            var category = search_by_categories ? CategoryNumber.ToString() : "";
            var search_parameters = new PaperSearch
            {
                regex = $@"(?is)(<\w+>)?.*({term}).*(<\w+>)?",
                category = category,
                limit = limit
            };

            var search_fn = async () => await driver.SearchNeo4J<Paper>(query, search_parameters);
            var pages = await search_fn.QuickWatch("pages speed");

            return Partial(partial_name, pages);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Partial("_Alert", new CustomAlert() { AlertType = AlertType.Error, Message = "Search failed!" });
        }
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

        try
        {
            var created = await driver.BulkCreateNodes<Paper>(query, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var customAlert = new CustomAlert()
            {
                Message = $"Failed to retrieve recommendations {e.Message}",
                AlertType = "error"
            };
            return Partial("_Alert", customAlert);
        }

        return Partial("_Alert", new CustomAlert()
        {
            Message = "<p class='' x-init='loading=false'>Done!</p>"
        });

        // return Content(alert("hey", "success"));
    }

    // public async Task<IActionResult> OnGetRecommendations()
    public async Task<IActionResult> OnPostRecommendations()
    {
        Console.WriteLine("Recommendations...".Dump());

        string query = await embeddedResourceQuery
            .GetQueryAsync<IndexModel>(new StackTrace());

        var is_query_empty = new Spec<string>(myquery => string.IsNullOrWhiteSpace(myquery));
        var has_no_recommendations = new Spec<Paper>(paper => string.IsNullOrWhiteSpace(paper.Url));

        query.Dump("query");
        if (is_query_empty.IsSatisfiedBy(query))
            return Partial("_Alert", new CustomAlert()
            {
                AlertType = AlertType.Warning,
                Message = "No query found.  Contact your admin."
            });

        var search_parameters = new PaperSearch
                {
                    // category = category,
                }
                .Dump()
            ;

        // search_parameters.Dump("s");
        var recommended_papers = await driver.SearchNeo4J<Paper>(query, search_parameters);
        recommended_papers.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Title)).Dump("recommendations");

        var users_who_liked_papers = await driver.SearchNeo4J<User>(query, search_parameters);
        users_who_liked_papers.FirstOrDefault(user => !string.IsNullOrWhiteSpace(user.last_name))
            .Dump("first user found")
            ;

        /*SELECT users.username, group_concat(movies.name), count(movies.name)
FROM user_fave_movies t1
INNER JOIN user_fave_movies t2 ON (t2.movie_id = t1.movie_id)
INNER JOIN users ON users.user_id = t2.user_id
INNER JOIN movies ON movies.id = t1.movie_id
WHERE t1.user_id = 1
AND t2.user_id <> 1
*/

        var common_likes = Enumerable
            .MaxBy(recommended_papers
                    .Where(p => !string.IsNullOrWhiteSpace(p.Title))
                , p => p.Title);

        //     from papers1 in recommended_papers
        //     join papers2 in recommended_papers on papers1.Title equals papers2.Title
        //     join user1 in users 
        // join users in users_who_liked_papers

        // on papers.Title = 

        // var recommended_by_users = await SearchNeo4J<UserRecommendedPapers>(query, search_parameters);
        // recommended_by_users.FirstOrDefault().Dump("recommended reading");

        // if (recommended_papers.Where(has_no_recommendations))
        //     return Partial("_Alert", new CustomAlert()
        //     {
        //         AlertCssClass = AlertType.Error,
        //         Message = "No query found.  Contact your admin."
        //     });

        // return Partial("_PaperList", recommended_papers);
        return Partial("_Modal", new CustomModal()
        {
//             Message = $"{users_who_liked_papers.Count} total Likes."
//                 .Prepend($"{recommended_papers.Count} total recommendations."),
//
//             Render = users_who_liked_papers
//                 .DistinctBy(x => x.last_name)
//                 .ToList()
//                 .Aggregate(new StringBuilder(), (sb, next_user) =>
//                 {
//                     // <button class='btn btn-accent'>View</button>
//                     sb.AppendLine($"""
//                                            <span class="card-title text-2xl"><b>Name: </b>{ next_user.FullName.Tag()}
//                                                         </span>
//                                            <span class=""><b>Age: </b>{ next_user.Age.ToString().Tag()} </span>
//                                            <span class=""><b>Email: </b>{ next_user.Email.Tag()} </span>
//                                    """ .Tag("div", className: "card-body")
//                     );
//                     return sb;
//                 })
//                 .ToString()
//                 // .AppendEach<User>(new List<User>(), () => "")
//                 // .FirstOrDefault()
//                 // .ToMaybe()
//                 // .IfSome(user => { return $"{user.last_name}, {user.first_name}".Tag("li"); })
//                 .Tag("ul", className: "card w-96 bg-base-100 shadow-xl")
//                 .Tag("div", className: "")
//                 .Tag("div", className: "flex flex-row items-center")
//                 .Prepend(
//                     new StringBuilder("Users liked the Paper entitled: '")
//                         .AppendEach(common_likes.AsList(),
//                             (shared_paper) => shared_paper.Title.Tag("h1", className: "text-lg text-secondary"))
//                         .Append("' ")
//                         .ToString()
//                         .Tag("div", className: "text-xl flex-row text-success max-w-128")
//                 )
//                 .AsHTMLString()
        });
    }

    public string? IsSelectedClassName(string panel_name, string cssClass)
    {
        return IsSelected(panel_name) ? cssClass : null;
    }
}


#region SCRAPS

// if (debug_mode)
//     partial_name.Dump("partial selected");


// string personal_access_token = Environment.GetEnvironmentVariable("TPOT_PAT");
// string tpot_base_key = Environment.GetEnvironmentVariable("TPOT_BASE_KEY");
//
// var airtable_query = @$"https://api.airtable.com/v0/{tpot_base_key}/Regex_Patterns?maxRecords=3&view=Grid%20view";  
//
// using HttpClient http_client = new HttpClient();
// http_client.DefaultRequestHeaders.Authorization =
//     new AuthenticationHeaderValue("Bearer", personal_access_token);


// var airtable_search = new AirtableSearch()
// {
//     table_name = "Regex_Patterns",
//     // filterByFormula = true.ToString(),
// };

// airtable_search.AsQuery().Dump("query");

// var regexes_from_airtable = await airtable_repo
// .SearchRecords<AirtableRegexPattern>(airtable_search, debug_mode: true);


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

#endregion SCRAPS