using CodeMechanic.Embeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Markdown;

public class IndexModel : PageModel
{
    private readonly IEmbeddedResourceQuery embeddedResourceQuery;
    private readonly IDriver driver;
    private static int count = 0;
    public int Count => count;

    public IndexModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver)

    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        this.driver = driver;
    }

    public void OnGet(
        string sort_direction = "..."
        , int max_lines = 3)
    {
        count = 0;
    }

    public async Task<IActionResult> OnPostStuff()
    {
        var failure_message = Content(
            $"""
            <div class='alert alert-error'>
              <p class='text-xl text-white text-sh'>An Error Occurred... But fret not! Our team of intelligent lab mice are on the
                job!</p>
            </div>
            """ );

        string result = "candy";
        string change = "coconut";

        return Content($"""
          <div>
            <h2>
            { result}  
              <span x-text='foo'  />
            </h2>

            <button class='btn btn-ghost' x-on:click='foo=`{ change}  `'>Change</button>          
          </div>
        """ );
    }
}