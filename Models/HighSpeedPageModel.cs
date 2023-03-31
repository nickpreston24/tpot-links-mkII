/* 
This class only exists to reduce boilerplate 

and because I can'think of a better name

 So, It's Hi-Speed.
 Inheritance sucks.

 Get used to it.

    TODO:

 - [ ] https://www.bytefish.de/blog/neo4j_dotnet.html

*/


using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CodeMechanic.Extensions;
using Neo4j.Driver;
using AirtableApiClient;
using CodeMechanic.Advanced.Extensions;
using CodeMechanic.Neo4j.Extensions;
using Page = TPOT_Links.Models.Page;
namespace CodeMechanic.RazorPages;

///<summary>
///Airtable https://github.com/ngocnicholas/airtable.net/wiki/Documentation
///</summary>
public abstract class HighSpeedPageModel : PageModel//, IQueryNeo4j, IQueryAirtable
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
        // , bool hydrate = false
    )
        where T : class, new()
    {
        var collection = new List<T>();
        
        if(parameters == null || string.IsNullOrWhiteSpace(query))
            return collection;

        // if(hydrate)
        //     query = query.Hydrate(parameters).Dump("hydrated query");

        await using var session = driver.AsyncSession();

        try
        {
            var results = await session.ExecuteReadAsync(async tx =>
            {
                var result = await tx.RunAsync(query, parameters.Dump("passed params"));
                return await result.ToListAsync<T>(record => {
                    
                    var node = record["page"].As<INode>();
                    var Id = node.Properties.Dump("properties")["Id"]?.As<long>().Dump("Id");
                    var Title = node.Properties["Title"]?.As<string>().Dump("Title");

                    // // var obj = new PropertyModel<IRecord>()
                    // // {
                    // //     GenericProperty = record
                    // // };
                    
                    // // //C# Extension Method: Object - GetPropertyValue
                    // // Console.WriteLine(obj.GetPropertyValue("GenericProperty").Dump("record so far"));
                    // // T obj = new T();
                    // var page = record.Values["page"].As<Page>();
                    // page.Dump("Page");
                    // // title.Dump("Found title");
                    // // record.Values["page"].Dump("i'm a record");
                    // // obj.GetType().GetProperties().Dump("obj props");
                    return new T();
                });
            });

             // var results = await session.ExecuteReadAsync(async tx =>
            // {
            //     var result = await tx.RunAsync(query, parameters.Dump("passed params"));
            //     // result.Dump("raw results");
            //     return await result.ToListAsync(record => {
            //         // var obj = record.Values["page"];//.Properties.Dump("i'm a record");
            //         // obj.GetType().GetProperties().Dump("obj props");
            //         return record;
            //     });
            // });

            // var results =  await session.RunAndConsumeAsync(query, parameters);
            // results.GetType().Dump("type");
            // return results.Dump("Results");

            // IResultCursor cursor = await session.RunAsync("MATCH (a:Person) RETURN a.name as name");
            // List<string> results = await cursor.ToListAsync(record => record["Title"].As<string>());

            // results.Dump("results");

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