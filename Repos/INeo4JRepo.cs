using Neo4j.Driver;
using TPOT_Links;

namespace CodeMechanic.Neo4j.Repos;

public interface INeo4JRepo
{
    Task<List<T>> SearchNeo4J<T>(
        string query
        , object parameters
        // , string label = "_fields"
        , Func<IRecord, T> mapper = null
        , bool debug_mode = false
    )
        where T : class, new();

    Task<List<T>> BulkCreateNodes<T>(
        string query
        // , string batch_command = ":param {}"
        , object parameters
        , Func<IRecord, T> mapper = null
        , bool debug_mode = false
    )
        where T : class, new();

    Task CreateIndices();
    // Task <List<T>> QueryAsync<T>();
    Task<IResultCursor> CreatePapers(List<Paper> papers);
}