using Microsoft.Data.SqlClient;

namespace TPOT_Links.Models;

public sealed class LogRow
{
    public int Id { get; set; } = -1;

    public string Table{ get; set; } 
    public string Server { get; set; } 
    public string Database { get; set; } 

    // e.g., the Stack Trace.
    public string ExceptionMessage { get; set; } 

   // Menu > Your SubMenu > bLah
    public string Breadcrumb { get; set; } 
    
    // updates { before: 'foo', after: 'bar', ... }
    public string Diff { get; set; } 

    // Use `.ToString()` and store.
    public SqlParameter[] RawValues { get; set; } = Array.Empty<SqlParameter>();

    // If it exists, log it.
    public string AssemblyName { get; set; } 

    // If it exists, log it.
    public string Namespace { get; set; } 

    /* Essential Auditing properties for tables */
    public DateTime DateModified { get; set; }
    public DateTime DateCreated { get; set; }

    // Person or program name
    public string LastModifiedBy { get; set; } 

    // Person or program name
    public string CreatedBy { get; set; } 

    /* 
      
      Optional Meta Properties

      These are highly flexible Rich Text (255->Max) fields, intended to catch varying bits of information that can be resolved in the App or simply viewed.
    */

    public string Url { get; set; }= string.Empty;// a Url for the Task, Story, Epic, number, or anything that is tracked through a Team's subversion or Team communication medium like Slack.

    public string Commit { get; set; }= string.Empty;// can be a URL or a Git/TFS commit #.

}
