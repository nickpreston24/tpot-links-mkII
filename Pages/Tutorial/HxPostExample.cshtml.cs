
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

    [BindProperty]
    public bool  show_slugs {get;set;} =  true;
    [BindProperty]

    public bool  show_excerpts {get;set;} =  true;
    [BindProperty]

    public bool  show_urls {get;set;} =  true;
    [BindProperty]
    public bool  case_insensitive {get;set;} =  true ;


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

        show_excerpts.Dump("show excerpts?");

        return Content($"""
            <div class="card lg:card-side bg-base-100 shadow-xl">
                <figure><img src="https://i.pravatar.cc/300" alt="Album" /></figure>
                <div class="card-body">
                    <h2 class="card-title">Hello, {Name}!</h2>
                    <p>You entered {Age} as your age, is this correct?</p>
                    <div class="card-actions justify-end">
                        <button class="btn btn-primary">Confirm</button>
                    </div>

                    <ul>
                        <label class="cursor-pointer label">
                            Show Excerpts?
                            <span class="label-text">{show_excerpts}</span>
                        </label>
                        <label class="cursor-pointer label">
                            Show Urls?
                            <span class="label-text">{show_urls}</span>
                        </label>
                        <label class="cursor-pointer label">
                            Case Insensitive?
                            <span class="label-text">{case_insensitive}</span>
                        </label>
                        <label class="cursor-pointer label">
                            Show Slugs?
                            <span class="label-text">{show_slugs}</span>
                        </label>
                    </ul>
                </div>
            </div>
        """);
    }

}


