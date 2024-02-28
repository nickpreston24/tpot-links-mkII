using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hydro;
using TPOT_Links.Models;

namespace TPOT_Links.Pages.Admin.Components;

public class Todos : HydroComponent
{
    public List<Todo> Items { get; set; } = new();

    [Required] public string NewItem { get; set; }

    public void Add()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        Items.Add(new Todo() { Content = NewItem });
        NewItem = string.Empty;
    }

    public void Toggle(string id)
    {
        var todo = Items.First(i => i.Id == id);
        todo.Done = !todo.Done;
    }
}