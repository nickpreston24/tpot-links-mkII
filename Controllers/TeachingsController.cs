using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.Neo4j.Repos;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TPOT_Links.Extensions;

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
    public async Task<IActionResult> Search(int limit = 5, string term = "Repentance", string category = "Chinese")
    {
        // string query = $"match (n) return n limit {limit}";
        // query.Dump(nameof(query));

        var readerfn = (() => embedService.GetFileContents<Pages.Sandbox.IndexModel>("SearchByRegex.cypher"));
        string query = readerfn.QuickWatch("read speed ");
        // query.Dump(nameof(query));

        var parameters = new PaperSearch()
        {
            regex = $@"(?is)(<\w+>)?.*({term}).*(<\w+>)?",
            category = category,
            limit = limit
        };

        var raw_paper_objects = await repo.SearchNeo4J<object>(query, parameters, debug_mode: false, mapper: record =>
        {
            var keys = record.Keys.Dump("keys");
            var paper = record[keys.FirstOrDefault()];
            return paper;
        });

        raw_paper_objects.Count.Dump("count of papers found: ");
        return Ok(raw_paper_objects);
    }

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