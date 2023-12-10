using CodeMechanic.Extensions;
using CodeMechanic.Types;
using Enumeration = CodeMechanic.Advanced.Extensions.Enumeration;

namespace TPOT_Links.Models;

public class AlertType : Enumeration
{
    public static AlertType Success => new(1, nameof(Success).ToLower());
    public static AlertType Warning => new(2, nameof(Warning).ToLower());
    public static AlertType Error => new(3, nameof(Error).ToLower());
    public static AlertType Info => new(4, nameof(Info).ToLower());

    public AlertType(int id, string name) : base(id, name)
    {
    }

    public static implicit operator AlertType(string name = "")
    {
        if (MissingExtensions.IsEmpty(name))
            name = Error.ToString();


        // TODO: getAll() is not getting all the fieldInfo[] and needs fixing.
        // var id_2 = GetAll<HxAttribute>()
        //     .FirstOrDefault(alert => alert.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        //     .Id;

        // NOTE: Temporary fix
        var lookup = new Dictionary<string, AlertType>()
        {
            { Success.Name, Success },
            { Error.Name, Error },
            { Warning.Name, Warning },
            { Info.Name, Info },
        };

        var _ = lookup.TryGetValue(name, out var found);
        // found.Dump("found alert :>> ");
        return found;
    }
}