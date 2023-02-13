
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
using TPOT_Links.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Comments;

public class ScriptureHighlighterModel : HighSpeedPageModel
{
    // 'static' allows us to holds the value between Crud calls.  We can do it with many different data types, too.
    private static int count = 0;
    public int Count => count; // making a non-static, public Count variable exposes the cahced value to our view.  Not many people use this, sadly.  It's great for reducing the number of calls to the server for data.

    public ScriptureHighlighterModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver) 
    : base(embeddedResourceQuery, driver)
    {
    }

    public void OnGet(
      string sort_direction="..."
      , int max_lines = 3)  // sample url parameters
    {
        // reset on refresh.  
        // Great for times when we want to just send url params in, without cached values.
        count = 0;
    }

    
    
    public async Task<IActionResult> OnPostExtractedScriptures()
    {
        // Renders an alert. Nice for when we implement global error handling, b/c this can be a component and can be handed a nice error message.
        var failure_message = Content(
            $"""
            <div class='alert alert-error'>
              <p class='text-xl text-white text-sh'>An Error Occurred... But fret not! Our team of intelligent lab mice are on the
                job!</p>
            </div>
            """);

        string result = "candy";
        string change = "coconut";

        /* 
          This demonstrates that we can apply updates to alpinejs variables in the cshtml file 
          from our server side templater.
        */

        return Content($"""
          <div>
            <h2>
            {result}
              <span x-text='foo'  />
            </h2>

            <button class='btn btn-ghost' x-on:click='foo=`{change}`'>Change</button>          
          </div>
        """);
    }

}


