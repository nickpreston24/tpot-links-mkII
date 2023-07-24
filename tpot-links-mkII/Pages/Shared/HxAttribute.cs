using CodeMechanic.Advanced.Extensions;

public class HxAttribute : Enumeration
{
    public static HxAttribute hx_get => new(1, nameof(hx_get));
    public static HxAttribute hx_post => new(2, nameof(hx_post));
    public static HxAttribute hx_put => new(2, nameof(hx_put));
    public static HxAttribute hx_delete => new(2, nameof(hx_delete));

    public HxAttribute(int id, string name) : base(id, name)
    {
    }

    public override string ToString()
    {
        return $"{Name.Replace("_", "-")} ";
    }
}