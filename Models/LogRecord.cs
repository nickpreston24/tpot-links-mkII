using Microsoft.Data.SqlClient;

namespace TPOT_Links;

public sealed record LogRecord
{
    public int Id { get; init; } = -1;

    public string Table { get; init; } = string.Empty;
    public string Server { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;

    // Use `.ToString()` and store.
    public IEnumerable<SqlParameter> RawValues { get; init; } = new List<SqlParameter>(0);

    // e.g., the Stack Trace.
    public string ExceptionMessage { get; init; } = string.Empty;

    // Menu > Your SubMenu > bLah
    public string Breadcrumb { get; init; } = string.Empty;

    // updates { before: 'foo', after: 'bar', ... }
    public string Diff { get; init; } = string.Empty;

    // If it exists, log it.
    public string AssemblyName { get; init; } = string.Empty;

    // If it exists, log it.
    public string Namespace { get; init; } = string.Empty;

    /* Essential Auditing properties for tables */
    public DateTime DateModified { get; init; } = default;

    public DateTime DateCreated { get; init; } = default;

    // Person or program name
    public string LastModifiedBy { get; init; } = string.Empty;

    // Person or program name
    public string CreatedBy { get; init; } = string.Empty;

    /* 
      
      Optional Meta Properties

      These are highly flexible Rich Text (255->Max) fields, intended to catch varying bits of information that can be resolved in the App or simply viewed.
    */

    public string Url { get; init; } =
        string.Empty; // a Url for the Task, Story, Epic, number, or anything that is tracked through a Team's subversion or Team communication medium like Slack.

    public string Commit { get; init; } = string.Empty; // can be a URL or a Git/TFS commit #.

    public override string ToString()
    {
        return base.ToString();
    }
}