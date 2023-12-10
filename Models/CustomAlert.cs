using TPOT_Links.Models;

public class CustomAlert
{
    public string Message { get; set; } = string.Empty;
    public AlertType AlertType { get; set; } = AlertType.Success;
}