using CodeMechanic.Diagnostics;
using CodeMechanic.Neo4j.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;
using TPOT_Links.Models;

namespace TPOT_Links.Pages.Sandbox;

[BindProperties]
public class AddNewPaper : PageModel
{
    private readonly INeo4JRepo neo4JRepo;
    private readonly IDriver driver;

    // [BindProperty]
    public Paper TpotPaper { get; set; } = new Paper();

    public AddNewPaper(
        INeo4JRepo neo4JRepo
        , IDriver driver
    )
    {
        this.neo4JRepo = neo4JRepo;
        this.driver = driver;
    }

    public void OnGet()
    {
        TpotPaper = new Paper()
        {
            Url = "https://www.thepathoftruth.com/teachings/repentance.htm",
            Title = "Repentance",
            Description = "test run"
        };
    }


    public async Task<IActionResult> OnGetCountNodes(string label = "Paper")
    {
        // nameof(OnGetCountNodes).Dump("running ");
        //
        string query = $"match (node:{label}) return node";
        // query.Dump();
        //
        var results = await neo4JRepo.SearchNeo4J<Paper>(query, null, debug_mode: false);
        // results.Dump();
        // return Content("Results: ", results.FirstOrDefault().Name);
        return Content($"Node Count : {results.Count}");
        // return Partial("_FriendsFound", results);
    }

    public async Task<IActionResult> OnDeleteRemovePaper()
    {
        string query = @"###### set (p:Paper {is_deleted: true}) return p "; // todo: add name to query 

        await using var session = driver.AsyncSession();

        var results = await session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync(query);
            result.Dump("raw write result");

            return await result.ToListAsync(record => record.MapToV2<Paper>());
        });

        return Content("Success! paper soft deleted!");
    }

    public async Task<IActionResult> OnPostAddNewPaper()
    {
        try
        {
            string query =
                @"merge (p:Paper {title:$title, description: $description, excerpt: $excerpt, guid: $guid}) return p ";
            
            var parameters = new
            {
                title = TpotPaper.Title,
                description = TpotPaper.Description,
                excerpt = TpotPaper.Excerpt ?? "",
                guid = TpotPaper.guid.ToString()
            };

            await using var session = driver.AsyncSession();

            var results = await session.ExecuteWriteAsync(async tx =>
            {
                var result = await tx.RunAsync(query, parameters);
                return await result.ToListAsync(record => record.MapToV2<Paper>());
            });

            return Content($"Success! {results.Count} new papers created!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Partial("_Alert", new CustomAlert() { Message = e.Message, AlertType = AlertType.Error });
        }
    }

    private async Task<List<Paper>> BulkUploadNewPapers(List<Paper> neo_papers)
    {
        neo_papers.Dump("bulk neo papers to upload");
        var parameters = new
        {
            batch = neo_papers.ToArray()
        };

        // https://stackoverflow.com/questions/69200606/merge-with-unwind-issue-neo4j
        // string query = """
        //                    WITH $batch AS batch
        //                    UNWIND batch as ind
        //                    MERGE (n:Paper{Title: ind.Title})
        //                    SET n += ind
        //                """;


        // string query = """
        //     MERGE (n:Friend{name: "Bucky Barnes", age: 37, hair_color: "Brown"})
        // """;

        // string query = "";
        // var created = await neo4JRepo.BulkCreateNodes<Paper>(query, parameters, debug_mode: true);
        // created.Dump("created");

        // return created.ToList();
        return default; // TODO: Idk why unwind refuses to work.  just use something else.
    }
}