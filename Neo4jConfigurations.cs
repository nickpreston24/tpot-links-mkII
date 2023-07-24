using Neo4j.Driver;

public static class Neo4jConfigurations
{
    public static void ConfigureNeo4j(this IServiceCollection services)
    {
        string uri = Environment.GetEnvironmentVariable("NEO4J_URI") ?? string.Empty;
        string user = Environment.GetEnvironmentVariable("NEO4J_USER") ?? string.Empty;
        string password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? string.Empty;

        services.AddSingleton(GraphDatabase.Driver(
            uri
            , AuthTokens.Basic(
                user,
                password
            )
        ));
    }
}