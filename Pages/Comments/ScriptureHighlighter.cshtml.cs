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
using CodeMechanic.Extensions;
using CodeMechanic.Advanced.Extensions;
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
      var failure_message = Content(
            $"""
            <div class='alert alert-error'>
              <p class='text-xl text-white text-sh'>An Error Occurred... But fret not! Our team of intelligent lab mice are on the
                job!</p>
            </div>
            """);

      var all_sample_text = sample_comments.Concat(sample_teachings);

      var scriptures = sample_comments.Select(comment=>comment.Extract<Scripture>());

      var regex_button = $"""
        <a href="https://regex101.com/r/HiM1uO/1">
          <button class="btn btn-primary">See the Regex</button>
        </a>
      """;      

      var result = Content($"""
        <div class="flex flex-col items-center justify-center">
          <div class="flex flex-center hero min-h-screen bg-base-200">
            <div class="hero-content flex-col lg:flex-row-reverse">
              <div class="flex flex-col items-center justify-center gap-2">
                <h1 class="text-5xl font-bold">Scripture Parsing</h1>
                <div class="flex flex-row items-center justify-center gap-2">
                  <div class="bg-secondary text-white-content mockup-code w-3/4">
                    <pre x-show="debug">
                        <template x-for="pattern in patterns">
                          <pre data-prefix="$">
                            <code x-text="pattern">
                            </code>
                          </pre> 
                      </template>
                    </pre>
                  </div>
                </div>

                <h2 class="text-4xl flex flex-col">
                  Comments Discovered
                  {regex_button}
                </h2>

                <div class="w-72 flex flex-col items-center justify-center gap-2">
                  <template x-for="comment in comment_highlights">
                    <h2 x-text="comment"></h2>
                  </template>
                </div>
              </div>
            </div>
          </div>
        </div>
      """);


       
       return result;
    }


    private static string[] sample_comments = new string [] {"Yes                        Victor Hafichuk . You speak The Truth!\n2 Corinthians 6:16 comes to mind! You must leave, separate yourselves from them, have nothing to do with what is unclean!", 
    
    "Embrace the Truth with your whole being. Romans 12:1,2 and Matthew 5:48. Flee all forms of religion, which exercises itself in every area of life - churchdom, government, medicine, entertainment, fashion, commerce, agriculture, food-production, processing, adulteration, and distribution.\n\nHebrews 4:12 - divide the good from the evil. Heed the Word of God. When we allow any pollution and corruption, we invite suffering, loss, and death. When we receive the Truth, we invite victory, security, healing, prosperity, peace, life, and joy. That's how it is. \n\nWhich do we choose and stand with - the prince of this world or the Prince of peace? The destroyer with all his machinations or the Savior in all His Glory? The one who seeks to deceive, steal, kill, and destroy us in all his devious, fleshly, and worldly ways, or the One Who laid His Life down for us so that we might, from the beginning to the end, have His Favor with Everlasting Life, Wisdom, and Power?\n\nIt doesn't take two brains to know the answer. In fact, we don't want two brains - it's called \"double-mindedness.\" One brain will do and only One, the Mind of Christ, our Creator and Lover. Amen?" };

    private static string [] sample_teachings = new string [] {""};

}


public class Scripture {

}

