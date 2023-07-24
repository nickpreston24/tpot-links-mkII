using Neo4j.Driver;

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

    Task CreateIndices();
    Task MergeCreateRelationship<T>(NodeBatch<T> batch_of_nodes = null);

    /// <summary>
    /// TODO: Move this to CodeMechanic!
    /// </summary>
    Task<IList<T>> BulkCreateNodes<T>(
        string query
        , object parameters = null
        , Func<IRecord, T> mapper = null
    )
        where T : class, new();
}