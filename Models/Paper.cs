using System.Reflection;
using System.Text.RegularExpressions;
using CodeMechanic.RegularExpressions;
using CodeMechanic.Diagnostics;
using CodeMechanic.Extensions;
using CodeMechanic.Types;
using StringExtensions = CodeMechanic.Types.StringExtensions;

namespace TPOT_Links;

public class Paper
{
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Categories { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    public string Rendered_Content { get; set; } = string.Empty;
    public string created { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Comment_status { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Links { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string last_modified_at_wp { get; set; } = string.Empty;
    public string created_at_wp { get; set; } = string.Empty;
    public string Type { get; set; }
    public int AuthorId { get; set; } = -1;

    // Where is the file now?  Neo4j? wordpress? etc...
    public string WebStatus { get; set; } = "Unknown";

    public Guid guid { get; set; } = new Guid();
}

public static class PaperExtensions
{
    private static Regex amps_pattern = new Regex(@"&.*;", RegexOptions.Compiled);

    private static Dictionary<string, string> replacement_map = new Dictionary<string, string>()
    {
        { @"&#8220;", "“" },
        { @"&eacute;", "é" },
        { @"&ntilde;", "ñ" },
        { @"&#8221;", "”" },
        { @"&iquest;", "¿" },
        { @"&oacute;", "ó" },
        { @"&ldquo;", "“" },
        { @"&rdquo;", "”" },
        { @"&#8217;", "’" },
        { @"&rsquo;", "’" }
    };

    public static Paper FixStrings(this Paper p)
    {
        p.Title = StringExtensions.FlattenText(p.Title
            .ToMaybe().IfNone(string.Empty)
            .AsArray().ReplaceAll(replacementMap: replacement_map));
        p.Excerpt = StringExtensions
            .FlattenText(p.Excerpt
                .ToMaybe().IfNone(string.Empty)
                .AsArray().ReplaceAll(replacementMap: replacement_map));
        return p;
    }

    public static int FindAmpIssues<T>(this T entity)
    {
        var str_props = typeof(T)
                .GetOnlyStringProps()
                .Select(prop => prop.GetValue(entity, null)?.ToString())
            // .Where(v=>v.ToString().NotEmpty())
            ;

        // Console.WriteLine("string props: "  + str_props.Count());
        // var amps_found = str_props?.Select(val => amps_pattern.Matches(val ?? "")).ToList();
        // return amps_found.Count;
        return 0;
    }

    public static IEnumerable<PropertyInfo> GetOnlyStringProps(this Type type)
    {
        return type.GetProperties().Where(prop => prop.PropertyType == typeof(string));
    }
}