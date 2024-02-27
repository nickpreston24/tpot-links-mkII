using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.Neo4j.Repos;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;

namespace TPOT_Links.Controllers;

public class TeachingsController : Controller
{
    private readonly INeo4JRepo repo;
    private readonly EmbeddedResourceService embedService;

    public TeachingsController(INeo4JRepo repo,
        IEmbeddedResourceQuery embeds
    )
    {
        this.repo = repo;
        embedService = embeds as EmbeddedResourceService;
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        int limit = 5
        , string term = "Repentance"
        , string category = "Chinese"
        , bool findupdates = false
    )
    {
        if (findupdates.Dump(nameof(findupdates)))
        {
            /* samples
                newer paper:     https://www.thepathoftruth.com/wp-admin/post.php?post=302308&action=edit
                older paper:   https://www.thepathoftruth.com/wp-admin/post.php?post=14907&action=edit
             */
            string find_updates_query = embedService.GetFileContents<TeachingsController>("SearchForOldPapers.cypher");
            Console.WriteLine("find updates query " + find_updates_query);
            return Ok(new Paper() { Description = "yup, this route worked" });
        }

        var readerfn = () => embedService.GetFileContents<Pages.Sandbox.IndexModel>("SearchByRegex.cypher");
        string paper_search_query = readerfn.QuickWatch(message: "read speed ");

        var parameters = new PaperSearch()
        {
            regex = $@"(?is)(<\w+>)?.*({term}).*(<\w+>)?",
            category = category,
            limit = limit
        };

        var raw_paper_objects = await repo.SearchNeo4J<object>(paper_search_query, parameters, debug_mode: false,
            mapper: record =>
            {
                var keys = record.Keys.Dump("keys");
                var paper = record[keys.FirstOrDefault()];
                return paper;
            });

        raw_paper_objects.Count.Dump("count of papers found: ");
        return Ok(raw_paper_objects);
    }


    // public IActionResult OnPostFindUpdates(WordpressPaper wp_paper)
    // {
    //     //TODO:
    //     // 1. Get 10 papers that haven't been updated in 30 days.
    //     // 2. Find the range of ids from lowest to highest and batch them.
    //     // 2. call neo4j for last update to a paper
    //     // 2. the wordpress api for new updates
    //     // 3. 
    //
    //     return Ok("Hello from " + nameof(OnPostFindUpdates));
    // }

    // [HttpGet]
    public IActionResult Top(int limit = 1)
    {
        limit.Dump(nameof(limit));
        var res = Enumerable.Repeat(new Paper()
        {
            Title = "Test",
            Description = "Your api is working."
        }, limit).AsList();


        return Ok(res);
    }
}

//
// public IActionResult Stuffies(int id)
// {
//     // return ControllerContext.MyDisplayRouteInfo(id);
//     id.Dump("'you rang?'");
//     var res = new Paper()
//     {
//         Title = "Test",
//         Description = "Your api is working."
//     }.AsList();
//     return Ok(res);
//     // return Ok("stuffies for your pup: " + id);
// }