using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using TPOT_Links.Models;

namespace TPOT_Links.Pages.Admin;
//Note: to remove all comments, replace this pattern with nothing:  // .*$

[BindProperties]
public class FeatureRequestModel : HighSpeedPageModel
{
    public Feature feature = new Feature();

    public FeatureRequestModel(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver)
        : base(embeddedResourceQuery, driver)
    {
    }

    public async Task<IActionResult> OnPostValidate(Feature feature)
    {
        (var Name, var Notes, var Url, _) = feature;

        // this.feature = feature;
        feature.Dump("feature");
        await Task.Run(() => Thread.Sleep(1500));

        return Content($"""
            <div 
                x-init='loading=false'
                class="card lg:card-side bg-base-100 shadow-xl">
                <div class="card-body">
                    <h2 class="card-title">Submit, {Name}!</h2>
                    <label>With Description:</label>
                    <textarea placeholder="Notes" disabled class="textarea textarea-bordered textarea-lg w-full max-w-xs" >{Notes}</textarea>

                    <a href={Url}>
                        <button>Go to Issue</button>
                    </a>
                    <div class="card-actions justify-end">
                        <button class="btn btn-primary">Confirm</button>
                    </div>
                </div>
            </div>
        """);
    }
}