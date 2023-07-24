using CodeMechanic.Advanced.Extensions;

public class AlertType : Enumeration
{
    public static AlertType Success => new(1, nameof(Success).ToLower());
    public static AlertType Warning => new(2, nameof(Warning).ToLower());
    public static AlertType Error => new(3, nameof(Error).ToLower());

    public AlertType(int id, string name)
        : base(id, name)
    {
    }
}