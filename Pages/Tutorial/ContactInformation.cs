namespace TPOT_Links.Pages.Tutorial;

public class ContactInformation
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; } = -1;
    public bool show_slugs { get; set; }
    public bool show_excerpts { get; set; }
    public bool show_urls { get; set; }
    public bool case_insensitive { get; set; }
    public bool search_by_categories { get; set; }
}