/* 
This class only exists to reduce boilerplate 

and because I can'think of a better name

 So, It's Hi-Speed.
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
using TPOT_Links.Extensions;
using Neo4j.Driver;

namespace TPOT_Links.RazorPages;

public abstract class HighSpeedPageModel : PageModel
{
    protected readonly IEmbeddedResourceQuery embeddedResourceQuery;
    protected readonly IDriver driver;

    public HighSpeedPageModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver
    )
    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        this.driver = driver;
    }

    public async Task<IList<IRecord>> NeoFind(string query, object parameters) 
    {
        var none = new List<IRecord>();
        
        if(parameters == null || string.IsNullOrWhiteSpace(query))
            return none;

        await using var session = driver
            .AsyncSession();
            // .AsyncSession(configBuilder => configBuilder
            // .WithDatabase("Recommendations"));

        try
        {
            var readResults = await session.ExecuteReadAsync(async tx =>
            {
                var result = await tx.RunAsync(query, parameters);
                return await result.ToListAsync();
            });

            return readResults;
        }
        
        // Capture any errors along with the query and data for traceability
        catch (Neo4jException ex)
        {
            Console.WriteLine($"{query} - {ex}");
            throw;
        }
    }
}