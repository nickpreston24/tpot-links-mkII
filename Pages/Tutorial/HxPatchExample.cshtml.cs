using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Tutorial;
//Note: to remove all comments, replace this with nothing:  // .*$

[BindProperties]
public class HxPatchExample : HighSpeedPageModel
{
    // public string? Name { get; init; } = string.Empty;
    // public int? Age { get; init; } = null!;
    // public bool show_slugs { get; set; }
    // public bool show_excerpts { get; set; }
    // public bool show_urls { get; set; }
    // public bool case_insensitive { get; set; }
    // public bool search_by_categories { get; set; }


    public HxPatchExample(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver)
        : base(embeddedResourceQuery, driver)
    {
    }

    public async Task<IActionResult> OnPatchUpdateRecord()
    {
        return Content($"""
            <p class="alert alert-success">
               DONE.
            </p>
        """);
    }
}