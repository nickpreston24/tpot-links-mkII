using System.Reflection;
using CodeMechanic.Extensions;
using Neo4j.Driver;

namespace CodeMechanic.Neo4j.Repos;

public static class UpdatedNeo4JExtensions
{
    /// <summary>
    /// PropertyCache stores the properties we wish to use again so we only have to run Reflection once per property.
    /// </summary>
    private static readonly IDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
        new Dictionary<Type, ICollection<PropertyInfo>>();

    public static T MapToV2<T>(
        this IRecord record
        , string label = ""
        , List<PropertyInfo> props = null
    )
        where T : class, new()
    {
        if (props == null) props = new List<PropertyInfo>();

        var type = typeof(T);

        // if no label provided, use the type's lowercased name
        label = string.IsNullOrEmpty(label)
            ? type.Name.ToLowerInvariant()
            : label;

        // if no props passed as an override, get them from the cache
        var properties = props.Count > 0
            ? props
            : _propertyCache.TryGetProperties<T>(true);

        // Neo4j nodes require labels
        if (!record.Keys.Contains(label))
            return new T();
        
        var node = record[label].As<INode>();

        if (properties.Count == 0)
        {
            return new T();
        }

        var obj = new T();

        foreach (var prop in properties ?? Enumerable.Empty<PropertyInfo>())
        {
            string name = prop.Name /*.Dump("key")*/;
            // var value = node.Properties[name].Dump("value");
            node.Properties.TryGetValue(name, out var value);

            var next_value = CreateSafeValue(value, prop);

            prop.SetValue(obj, next_value /*.Dump("value")*/, null);
        }

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