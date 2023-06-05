/* 
This class only exists to reduce boilerplate 

and because I can'think of a better name

 So, It's Hi-Speed.
 Inheritance sucks.

 Get used to it.

    TODO:

 - [ ] https://www.bytefish.de/blog/neo4j_dotnet.html

*/


using CodeMechanic.Embeds;
using CodeMechanic.Neo4j.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

namespace CodeMechanic.RazorPages;

///<summary>
///Airtable https://github.com/ngocnicholas/airtable.net/wiki/Documentation
///</summary>
public abstract class HighSpeedPageModel : PageModel //, IQueryNeo4j, IQueryAirtable
{
    protected readonly IEmbeddedResourceQuery embeddedResourceQuery;
    protected readonly IDriver driver;
    protected readonly IAirtableRepo airtable_repo;

    public HighSpeedPageModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver = null
        , IAirtableRepo repo = null
    )
    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        this.driver = driver;
        this.airtable_repo = repo;
    }

    public async Task<IList<T>> SearchNeo4J<T>(
        string query
        , object parameters
        , Func<IRecord, T> mapper = null
    )
        where T : class, new()
    {
        // parameters.Dump("Hello from params");

        if (mapper == null)
            mapper = record => record.MapTo<T>();

        var collection = new List<T>();

        if (parameters == null || string.IsNullOrWhiteSpace(query))
            return collection;

        await using var session = driver.AsyncSession();

        try
        {
            var results = await session.ExecuteReadAsync(async tx =>
            {
                var result = await tx.RunAsync(query, parameters);
                return await result.ToListAsync<T>(mapper);
            });

            return results;
        }

        // Capture any errors along with the query and data for traceability
        catch (Neo4jException ex)
        {
            Console.WriteLine($"{query} - {ex}");
            throw;
        }
        finally
        {
            session.CloseAsync();
        }
    }

    public async Task<IList<T>> BulkCreateNodes<T>(
        string query = ""
        // , string batch_command = ":param {}"
        , object parameters = null
        , Func<IRecord, T> mapper = null
    ) 
        where T : class, new()
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        
        await using var session = driver.AsyncSession();
        
        try
        {
            var results = await session.ExecuteWriteAsync(async tx =>
            {
                // var _ = await tx.RunAsync(batch_command, null);
                var result = await tx.RunAsync(query, parameters);
                return await result.ToListAsync<T>(record => record.MapTo<T>());
            });

            return results;
        }
        
        // Capture any errors along with the query and data for traceability
        catch (Neo4jException ex)
        {
            Console.WriteLine($"{query} - {ex}");
            throw;
        }
        finally {
            session.CloseAsync();
        }
    }


    // public object NeoWrite(string query, IDictionary<string, object> neo4j_params) 
    // {
    //     await using var session = driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));

    //     try
    //     {
    //         // Write transactions allow the driver to handle retries and transient error
    //         var writeResults = await session.ExecuteWriteAsync(async tx =>
    //         {
    //             var result = await tx.RunAsync(query, 
    //             new { 
    //                 person1Name, person2Name 
    //             });
    //             return await result.ToListAsync();
    //         });

    //         // foreach (var result in writeResults)
    //         // {
    //         //     var person1 = result["p1"].As<INode>().Properties["name"];
    //         //     var person2 = result["p2"].As<INode>().Properties["name"];
    //         //     Console.WriteLine($"Created friendship between: {person1}, {person2}");
    //         // }
    //     }

    //     // Capture any errors along with the query and data for traceability
    //     catch (Neo4jException ex)
    //     {
    //         Console.WriteLine($"{query} - {ex}");
    //         throw;
    //     }
    // }
}