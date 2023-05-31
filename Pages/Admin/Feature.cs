namespace TPOT_Links.Models;

public class Feature
{
    public void Deconstruct(out string name, out string notes, out string url, out string status)
    {
        name = Name;
        notes = Notes;
        url = Url;
        status = Status;
    }

    public string Name { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Status { get; set; } = "Requested";
}