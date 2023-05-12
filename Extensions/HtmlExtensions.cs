using CodeMechanic.Extensions;
namespace TPOT_Links;

public static class HtmlExtensions 
{
    private const string fallback = "p";
    /// <summary>
    /// Surrounds Text with html tag.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static string Tag(this string value
    , string tag = fallback
    , string className = ""
    , params string[] attributes
    )
    {
        var attr = !attributes.IsNullOrEmpty() ? attributes.FlattenText() : "";

        string end_tag =
            !tag
            .ToLowerInvariant()
            .Contains("input") ? $"</{tag}>" : $"{tag}>";

        //return new StringBuilder($"<{tag}>{value}")
        //    .Append(end_tag).ToString();

        var html = (!string.IsNullOrWhiteSpace(className))
            ? $"<{tag} class='{className}' {attr}>{value}{end_tag}"
            : $"<{tag}{attr}>{value}{end_tag}";

        return html;
    }
}
