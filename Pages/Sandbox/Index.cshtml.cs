
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
using CodeMechanic.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Sandbox;

public class IndexModel : HighSpeedPageModel
{

    private static int count = 0;

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver) 
    : base(embeddedResourceQuery, driver)
    {
    }

    public void OnGet()
    {
        // reset on refresh
        count = 0;
    }
  
    public async Task<IActionResult> OnGetRecommendations()
    {
        var failure = Content(
        $"<div class='alert alert-error'><p class='text-3xl text-warning text-sh'>An Error Occurred...  But fret not! Our team of intelligent lab mice are on the job!</p></div>");

        string query = "...";

        // Magically infers that the current method name is referring to 'Recommended.cypher'
        var trace = new StackTrace();
        query = await embeddedResourceQuery.GetQueryAsync<IndexModel>(trace);

        if(string.IsNullOrEmpty(query))
            return failure;

        // This can also be a template
        return Content(
            $"<div class='alert alert-primary'><p class='text-xl text-secondary text-sh'>{query}</p></div>");
    }

}


