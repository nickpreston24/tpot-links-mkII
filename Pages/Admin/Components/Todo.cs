namespace TPOT_Links.Models;

public class Todo
{
    public string Id { get; set; } = ""; //Guid.NewGuid().ToString("N");
    public string Content { get; set; } = "";
    public bool Done { get; set; } = false;
}