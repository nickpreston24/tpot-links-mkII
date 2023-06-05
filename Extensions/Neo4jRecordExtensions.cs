using System.Reflection;
using System.Text;
using CodeMechanic.Reflection;
using Neo4j.Driver;

namespace CodeMechanic.Neo4j.Extensions;

public static class Neo4jRecordExtensions
{
    /// <summary>
    /// PropertyCache stores the properties we wish to use again so we only have to run Reflection once per property.
    /// </summary>
    private static readonly IDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
        new Dictionary<Type, ICollection<PropertyInfo>>();

    /// <summary>
    /// For use in seeing what the full query will look like when all $vars are injected.
    /// </summary>
    public static string GetHydratedQuery(this string original, params object[] neo_params)
    {
        if (string.IsNullOrWhiteSpace(original))
            original = "MATCH (n) return n";

        var all_hydratable_variables = neo_params
            .SelectMany(parameter =>
            {
                var dictionary = GetObjectKeyValuePairs(parameter);
                return dictionary;
            });

        string result = new StringBuilder().ToString();


        return result;
    }


    internal static Dictionary<string, object> GetObjectKeyValuePairs(object arg)
    {
        // ICollection<PropertyInfo> properties = _propertyCache
        //     .TryGetProperties(
        //         
        //          // TODO: finish
        //         );
        var props = arg.GetType().GetProperties();
        var result = props
            .ToDictionary(property => property.Name, property => property.GetValue(arg));

        return result;
    }


    public static T MapTo<T>(this IRecord record
        , string label = ""
    )
        where T : class, new()
    {
        var type = typeof(T);
        label = string.IsNullOrWhiteSpace(label)
            ? type.Name.ToLowerInvariant()
            : label;

        ICollection<PropertyInfo> properties = _propertyCache
            .TryGetProperties<T>(true);

        if (properties.Count == 0)
        {
            return new T();
        }

        var obj = new T();

        foreach (var prop in properties ?? Enumerable.Empty<PropertyInfo>())
        {
            string name = prop.Name /*.Dump("key")*/;
            // var value = node.Properties[name].Dump("value");
            var node = record /*.Dump("raw record")*/[label].As<INode>();

            node.Properties.TryGetValue(name, out var value);

            var next_value = CreateSafeValue(value, prop);

            prop.SetValue(obj, next_value /*.Dump("value")*/, null);
        }

        // obj.Dump("T's obj");

        return obj;
    }

    private static object CreateSafeValue(object value, PropertyInfo prop)
    {
        Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

        object safeValue =
            value == null
                ? null
                : Convert.ChangeType(value, propType);

        return safeValue;
    }
}