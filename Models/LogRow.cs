using Microsoft.Data.SqlClient;

namespace TPOT_Links.Models;

sealed class LogRow
{
    public int Id { get; set; } = -1;

    public string Table{ get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;

    // e.g., the Stack Trace.
    public string ExceptionMessage { get; set; } = string.Empty;

   // Menu > Your SubMenu > bLah
    public string Breadcrumb { get; set; } = string.Empty;
    
    // updates { before: 'foo', after: 'bar', ... }
    public string Diff { get; set; } = string.Empty;

    // Use `.ToString()` and store.
    public SqlParameter[] RawValues { get; set; } = Array.Empty<SqlParameter>();

    // If it exists, log it.
    public string AssemblyName { get; set; } = string.Empty;

    // If it exists, log it.
    public string Namespace { get; set; } = string.Empty;

    /* Essential Auditing properties for tables */
    public DateTime DateModified { get; set; }
    public DateTime DateCreated { get; set; }

    // Person or program name
    public string LastModifiedBy { get; set; } = string.Empty;

    // Person or program name
    public string CreatedBy { get; set; } = string.Empty;

    /* 
      
      Optional Meta Properties

      These are highly flexible Rich Text (255->Max) fields, intended to catch varying bits of information that can be resolved in the App or simply viewed.
    */

    public string Url { get; set; } = string.Empty; // a Url for the Task, Story, Epic, number, or anything that is tracked through a Team's subversion or Team communication medium like Slack.

    public string Commit { get; set; } = string.Empty; // can be a URL or a Git/TFS commit #.

}
