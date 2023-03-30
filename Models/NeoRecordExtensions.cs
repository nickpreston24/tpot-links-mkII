using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Neo4j.Driver;
using Neo4j.Driver.Internal.Types;
using TPOT_Links.Models;

namespace CodeMechanic.Extensions;

public static class NeoRecordExtensions 
{
    /// <summary>
    /// PropertyCache stores the properties we wish to use again so we only have to run Reflection once per property.
    /// </summary>
    private static readonly IDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
        new Dictionary<Type, ICollection<PropertyInfo>>();

    public static IList<T> MapTo<T>(this IList<IRecord> neo_records) 
        where T : class, new()
    {
        var collection = new List<T>();
        // var properties = typeof(T).GetProperties();

        ICollection<PropertyInfo> properties = _propertyCache
                    .TryGetProperties<T>(true);

        properties.Count.Dump("total properties");
        foreach (IRecord record in neo_records)
        {
            // record.Dump("record");
            var node = record.Values["page"].As<Paper>().Dump("node");
            // var raw_values = record.Values["page"].GetType().Dump("vals raw");

            // string status = record["Status"].As<string>();
            // status.Dump("status");

            T obj = new T();

            // raw_values.Keys.Dump("keys");

            // foreach (PropertyInfo prop in properties)
            // {
            //     try
            //     {
            //         object value = raw_values[prop.Name];
            //         // value.Dump("new value");

            //         prop.SetValue(obj, value, null);
            //     }
            //     catch
            //     {
            //         continue;
            //     }
            // }

            collection.Add(obj);

        }


        return collection;
    } 
}
