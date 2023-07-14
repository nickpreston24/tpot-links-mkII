using CodeMechanic.Advanced.Extensions;

public class AlertType : Enumeration
{
    public static AlertType Success => new(1, nameof(Success));
    public static AlertType Warning => new(1, nameof(Warning));
    public static AlertType Fail => new(2, nameof(Fail));

    public AlertType(int id, string name)
        : base(id, name)
    {
    }
}