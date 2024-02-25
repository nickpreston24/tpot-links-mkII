using System.Text;

namespace TPOT_Links.Pages.Sandbox;

public class Friend
{
    public string name { get; set; } = String.Empty;
    public string age { get; set; } = String.Empty;
    public string hair_color { get; set; } = String.Empty;

    public override string ToString()
    {
        return
            new StringBuilder()
                .AppendLine($"<b>{nameof(name)}</b>: {name}".Tag("li"))
                .AppendLine($"<b>{nameof(age)}</b>: {age}".Tag("li"))
                .AppendLine($"<b>{nameof(hair_color)}</b>: {hair_color}".Tag("li"))
                .ToString()
            ;
    }
}