using System.Text;
using CodeMechanic.Diagnostics;
using Neo4j.Driver;
using TPOT_Links;
using TPOT_Links.Models;
using Category = Neo4j.Driver.Category;

// using Category = TPOT_Links.Category;

namespace CodeMechanic.Neo4j.Repos;

public interface INeo4JRepo
{
    Task<IList<T>> SearchNeo4J<T>(
        string query
        , object parameters
        // , string label = "_fields"
        , Func<IRecord, T> mapper = null
        , bool debug_mode = false
    )
        where T : class, new();

    Task<IList<T>> BulkCreateNodes<T>(
        string query = ""
        // , string batch_command = ":param {}"
        , object parameters = null
        , Func<IRecord, T> mapper = null
    )
        where T : class, new();

    Task CreateIndices();
}
//
// public class Neo4JRepo : IDisposable, INeo4JRepo
// {
//     private readonly IDriver driver;
//
//     public Neo4JRepo(
//         IDriver driver
//         // , IConnectionSettings settings
//     )
//     {
//         string uri = Environment.GetEnvironmentVariable("NEO4J_URI") ?? string.Empty;
//         string user = Environment.GetEnvironmentVariable("NEO4J_USER") ?? string.Empty;
//         string password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? string.Empty;
//         //Doing this because I keep disposing driver
//         this.driver = GraphDatabase.Driver(
//             uri
//             , AuthTokens.Basic(
//                 user,
//                 password
//             )
//         );
//         // this.driver = driver;
//         // driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
//     }
//
//
//     public async Task<IList<T>> SearchNeo4J<T>(
//         string query
//         , object parameters
//         // , string label = "_fields"
//         , Func<IRecord, T> mapper = null
//         , bool debug_mode = false
//     )
//         where T : class, new()
//     {
//         // parameters.Dump("Hello from params");
//
//         if (mapper == null)
//             mapper = delegate(IRecord record)
//             {
//                 if (debug_mode)
//                 {
//                     record.Values.Dump("record values");
//                 }
//
//                 return record.MapTo<T>();
//             };
//
//         var collection = new List<T>();
//
//         if (parameters == null || string.IsNullOrWhiteSpace(query))
//             return collection;
//
//         await using var session = driver.AsyncSession();
//
//         try
//         {
//             var results = await session.ExecuteReadAsync(async tx =>
//             {
//                 var result = await tx.RunAsync(query, parameters);
//                 return await result.ToListAsync<T>(mapper);
//             });
//
//             return results;
//         }
//
//         // Capture any errors along with the query and data for traceability
//         catch (Neo4jException ex)
//         {
//             Console.WriteLine($"{query} - {ex}");
//             throw;
//         }
//         finally
//         {
//             session.CloseAsync();
//         }
//     }
//
//     public async Task<IList<T>> BulkCreateNodes<T>(
//         string query = ""
//         // , string batch_command = ":param {}"
//         , object parameters = null
//         , Func<IRecord, T> mapper = null
//     )
//         where T : class, new()
//     {
//         if (parameters == null) throw new ArgumentNullException(nameof(parameters));
//
//         await using var session = driver.AsyncSession();
//
//         try
//         {
//             var results = await session.ExecuteWriteAsync(async tx =>
//             {
//                 // var _ = await tx.RunAsync(batch_command, null);
//                 var result = await tx.RunAsync(query, parameters);
//                 return await result.ToListAsync<T>(record => record.MapTo<T>());
//             });
//
//             return results;
//         }
//
//         // Capture any errors along with the query and data for traceability
//         catch (Neo4jException ex)
//         {
//             Console.WriteLine($"{query} - {ex}");
//             throw;
//         }
//         finally
//         {
//             session.CloseAsync();
//         }
//     }
//
//
//     public async Task CreateIndices()
//     {
//         string[] queries =
//         {
//             "CREATE INDEX ON :Page(title)",
//             "CREATE INDEX ON :Page(id)",
//             "CREATE INDEX ON :Person(id)",
//             "CREATE INDEX ON :Person(name)",
//             "CREATE INDEX ON :Category(name)"
//         };
//         await using var session = driver.AsyncSession();
//
//         foreach (var query in queries)
//         {
//             await session.RunAsync(query);
//         }
//     }
//
//     public async Task CreatePersons(List<Person> persons)
//     {
//         string cypher = new StringBuilder()
//             .AppendLine("UNWIND {persons} AS person")
//             .AppendLine("MERGE (p:Person {name: person.name})")
//             .AppendLine("SET p = person")
//             .ToString();
//
//         await using var session = driver.AsyncSession();
//         await session.RunAsync(cypher,
//             new Dictionary<string, object>()
//             {
//                 {
//                     "persons",
//                     ParameterSerializer
//                     // CodeMechanic.Neo4j.Extensions.ParameterSerializer.ToDictionary(persons)
//                 }
//             });
//     }
//
//     public async Task CreateCategories(IList<Category> categories)
//     {
//         string cypher = new StringBuilder()
//             .AppendLine("UNWIND {categories} AS category")
//             .AppendLine("MERGE (g:Category {name: category.name})")
//             .AppendLine("SET g = category")
//             .ToString();
//
//         await using var session = driver.AsyncSession();
//         await session.RunAsync(cypher,
//             new Dictionary<string, object>()
//             {
//                 {
//                     "categories", ParameterSerializer.ToDictionary(categories)
//                 }
//             });
//     }
//
//     public async Task CreatePages(IList<Paper> pages)
//     {
//         string cypher = new StringBuilder()
//             .AppendLine("UNWIND {pages} AS page")
//             .AppendLine("MERGE (m:Page {id: page.id})")
//             .AppendLine("SET m = page")
//             .ToString();
//
//         await using var session = driver.AsyncSession();
//
//         await session.RunAsync(cypher,
//             new Dictionary<string, object>()
//             {
//                 {
//                     "pages", ParameterSerializer.ToDictionary(pages)
//                 }
//             });
//     }
//
//     // public async Task UpdateExistingNodeBatch<T>(NodeBatch<T> batch_of_nodes = null)
//     // {
//     //     // { batch : [{"1":334,"2":222,3:3840, ... 100k}]}
//     //     string query = """
//     //                     WITH $batch as data, [k in keys($batch) | toInteger(k)] as ids
//     //                     MATCH (n) WHERE id(n) IN ids
//     //                     // single property value
//     //                     SET n.count = data[toString(id(n))]
//     //                     // or override all properties
//     //                     SET n = data[toString(id(n))]
//     //                     // or add all properties
//     //                     SET n += data[toString(id(n))]
//     //                     """;
//     //
//     //     return;
//     // }
//
//     public async Task MergeCreateRelationship<T>(NodeBatch<T> batch_of_nodes = null)
//     {
//         // {batch: [
//         //     {from:"alice@example.com",to:"bob@example.com",properties:{since:2012}},{from:"alice@example.com",to:"charlie@example.com",properties:{since:2016}}]}
//         var query = """
//         UNWIND $batch as row
//         MATCH (from:Label {id: row.from})
//         MATCH (to:Label {id: row.to})
//         CREATE/MERGE (from)-[rel:KNOWS]->(to)
//             (ON CREATE) SET rel += row.properties
//         """;
//
//         return;
//     }
//
//
//     // public async Task CreateRelationships(IList<MovieInformation> metadatas)
//     // {
//     //     string cypher = new StringBuilder()
//     //         .AppendLine("UNWIND {metadatas} AS metadata")
//     //         // Find the Page:
//     //             .AppendLine("MATCH (m:Page { title: metadata.page.title })")
//     //             // Create Cast Relationships:
//     //             .AppendLine("UNWIND metadata.cast AS actor")   
//     //             .AppendLine("MATCH (a:Person { name: actor.name })")
//     //             .AppendLine("MERGE (a)-[r:ACTED_IN]->(m)")
//     //             // Create Director Relationship:
//     //             .AppendLine("WITH metadata, m")
//     //             .AppendLine("MATCH (d:Person { name: metadata.director.name })")
//     //             .AppendLine("MERGE (d)-[r:DIRECTED]->(m)")
//     //         // Add Categories:
//     //         .AppendLine("WITH metadata, m")
//     //         .AppendLine("UNWIND metadata.categories AS category")
//     //         .AppendLine("MATCH (g:Category { name: category.name})")
//     //         .AppendLine("MERGE (m)-[r:GENRE]->(g)")
//     //         .ToString();
//
//     //     await using var session = driver.AsyncSession();
//
//     //     {
//     //         await session.RunAsync(cypher, new Dictionary<string, object>() { { "metadatas", ParameterSerializer.ToDictionary(metadatas) } });
//     //     }
//     // }
//
//     public void Dispose()
//     {
//         driver?.Dispose();
//     }
// }
//
// public sealed class NodeBatch<T>
// {
//     public NodeBatch(params T[] items)
//     {
//     }
//
//     public override string ToString()
//     {
//         return base.ToString();
//     }
// }