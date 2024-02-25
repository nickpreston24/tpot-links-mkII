namespace TPOT_Links;

public class PaperSearch
{
    public string id { get; set; } = 1.ToString();
    public int limit { get; set; } = 100;

    public string title { get; set; } = string.Empty;

    // public string term { get; set; } = string.Empty;
    public string regex { get; set; } = string.Empty;
    public string category { get; set; } = string.Empty;
}