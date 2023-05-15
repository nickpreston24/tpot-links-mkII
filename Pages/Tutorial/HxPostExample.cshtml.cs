
using System.Diagnostics;
using System.Text;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.RazorPages;
using CodeMechanic.Types;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using TPOT_Links.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace TPOT_Links.Pages.Tutorial;
//Note: to remove all comments, replace this with nothing:  // .*$

public class HxPostExample : HighSpeedPageModel
{

    [BindProperty, Required]
    public string? Name { get; init; } = string.Empty;

    [BindProperty, Required]
    public int? Age { get; init; } = null!;

    public HxPostExample(
        IEmbeddedResourceQuery embeddedResourceQuery
        , IDriver driver) 
    : base(embeddedResourceQuery, driver)
    {
    }

    public async Task<IActionResult> OnPostValidate()
    {
        "hello from validate".Dump();
        Name.Dump();
        Age.Dump();
        return Content($"""
            <div class="card lg:card-side bg-base-100 shadow-xl">
                <figure><img src="https://i.pravatar.cc/300" alt="Album" /></figure>
                <div class="card-body">
                    <h2 class="card-title">Hello, {Name}!</h2>
                    <p>You entered {Age} as your age, is this correct?</p>
                    <div class="card-actions justify-end">
                        <button class="btn btn-primary">Confirm</button>
                    </div>
                </div>
            </div>


        """);
    }

}


