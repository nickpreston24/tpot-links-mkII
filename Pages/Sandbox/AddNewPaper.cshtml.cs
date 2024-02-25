using CodeMechanic.Diagnostics;
using CodeMechanic.Neo4j.Repos;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

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
        var results = await neo4JRepo.SearchNeo4J<Paper>(query, null, debug_mode: true);
        // results.Dump();
        // return Content("Results: ", results.FirstOrDefault().Name);
        return Content($"Node Count : {results.Count}");
        // return Partial("_FriendsFound", results);
    }

    public async Task<IActionResult> OnPostAddNewPaper()
    {
        string query = @"merge (p:Paper {Title:$title, description: $description}) return p ";
        var parameters = new 
        {
            title = TpotPaper.Title,
            description = TpotPaper.Description
        };

        await using var session = driver.AsyncSession();

        var results = await session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync(query, parameters);
            result.Dump("raw write result");

            return await result.ToListAsync(record => record.MapToV2<Paper>());
        });
       
        return Content("Success! new papers created!");
    }

    private async Task<List<Paper>> BulkUploadNewPapers(List<Paper> neo_papers)
    {
        neo_papers.Dump("all neo papers");
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