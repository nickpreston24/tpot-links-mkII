namespace TPOT_Links.Models;

public class CardOptions
{
    public bool show_slugs { get; set; } = true;
    public bool show_excerpts { get; set; }
    public bool show_urls { get; set; }
    public bool case_insensitive { get; set; }
    public bool dev_mode { get; set; } = true;  
}