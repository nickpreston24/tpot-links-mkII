using System.Text;
using CodeMechanic.Extensions;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace TPOT_Links;

public static class HtmlExtensions
{
    private const string fallback = "p";
    public static string AsJS<T>(this T obj) => JsonConvert.SerializeObject(obj);


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
                .Contains("input")
                ? $"</{tag}>"
                : $"{tag}>";

        //return new StringBuilder($"<{tag}>{value}")
        //    .Append(end_tag).ToString();

        var html = (!string.IsNullOrWhiteSpace(className))
            ? $"<{tag} class='{className}' {attr}>{value}{end_tag}"
            : $"<{tag}{attr}>{value}{end_tag}";

        return html;
    }

    /// <summary>
    /// Quick wrapper for creating safe HTML strings on the backend.
    /// </summary>
    public static HtmlString AsHTMLString(this string raw_text)
    {
        var clone = new StringBuilder(raw_text); // lazy clone.
        var result = new HtmlString(clone.ToString());
        return result;
    }

    // public static IActionResult RenderHtml<T>(this string html) => Content(html.ToString(), "text/html");
    // public static Func<T, IActionResult> CreateRender<T>(this string html)
    // {
    //     // return Microsoft.AspNetCore.Mvc.RazorPages.Content(html, null);
    //     return null;
    // }
}