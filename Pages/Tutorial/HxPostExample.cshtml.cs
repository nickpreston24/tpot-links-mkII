using CodeMechanic.Embeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4j.Driver;

namespace TPOT_Links.Pages.Tutorial;
//Note: to remove all comments, replace this pattern with nothing:  // .*$

[BindProperties]
public class HxPostExample: PageModel
{
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; } = -1;
    public bool show_slugs { get; set; }
    public bool show_excerpts { get; set; }
    public bool show_urls { get; set; }
    public bool case_insensitive { get; set; }
    public bool search_by_categories { get; set; }


    public HxPostExample(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver)
        
    {
    }

//     public async Task<IActionResult> OnPostValidate([FromBody] ContactInformation contact_info)
//     {
//         Name.Dump("your name is");
//         contact_info.Dump("OLD");
//         return Content($"""
//                             <div class="card lg:card-side bg-base-100 shadow-xl">
//                                 <figure><img src="https://i.pravatar.cc/300" alt="Album" /></figure>
//                                 <div class="card-body">
//                                     <h2 class="card-title">Hello, {Name}!</h2>
//                                     <p>You entered {Age} as your age, is this correct?</p>
//                                     <div class="card-actions justify-end">
//                                         <button class="btn btn-primary">Confirm</button>
//                                     </div>
//                         
//                                     <ul>
//                                         <label class="cursor-pointer label">
//                                             Show Excerpts?
//                                             <span class="label-text">{show_excerpts}</span>
//                                         </label>
//                                         <label class="cursor-pointer label">
//                                             Show Urls?
//                                             <span class="label-text">{show_urls}</span>
//                                         </label>
//                                         <label class="cursor-pointer label">
//                                             Case Insensitive?
//                                             <span class="label-text">{case_insensitive}</span>
//                                         </label>
//                                         <label class="cursor-pointer label">
//                                             Show Slugs?
//                                             <span class="label-text">{show_slugs}</span>
//                                         </label>
//                         
//                                         <label class="cursor-pointer label">
//                                             Search By Categories?
//                                             <span class="label-text">{search_by_categories}</span>
//                                         </label>
//                                     </ul>
//                                 </div>
//                             </div>
//                         """);
//     }

    
}