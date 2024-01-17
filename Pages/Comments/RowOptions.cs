namespace TPOT_Links.Models;

public class RowOptions
{
    public string[] rows_counts = new[] { "Max rows", "10", "20" };
    public string handler { get; set; } // for use as the page-handler with htmx or razor.
    public string page { get; set; } = "Index"; // for use with htmx
}