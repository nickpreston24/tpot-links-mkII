using CodeMechanic.Neo4j.Repos;
using CodeMechanic.Types;
using Neo4j.Driver;

public static class Neo4jConfigurations
{
    public static void ConfigureNeo4j(this IServiceCollection services
        , Neo4jConfig config
    )
    {
        var driver = GraphDatabase.Driver(
            config.uri
            , AuthTokens.Basic(
                config.user,
                config.password
            )
        );
        services.AddSingleton(
            driver
        );

        services.AddSingleton<INeo4JRepo>(new Neo4JRepo(driver, config));
    }
}


public class Neo4jConfig
{
    public static Neo4jConfig GetConfig(bool use_localhost = false)
    {
        return !use_localhost
            ? new Neo4jConfig()
            {
                uri = Environment.GetEnvironmentVariable("NEO4J_URI") ?? string.Empty,
                user = Environment.GetEnvironmentVariable("NEO4J_USER") ?? string.Empty,
                password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? string.Empty,
                dbname = Environment.GetEnvironmentVariable("NEO4J_DBNAME") ?? string.Empty
            }
            : new Neo4jConfig()
            {
                uri = Environment.GetEnvironmentVariable("NEO4J_LOCAL_URI") ?? string.Empty,
                user = Environment.GetEnvironmentVariable("NEO4J_LOCAL_USER") ?? string.Empty,
                password = Environment.GetEnvironmentVariable("NEO4J_LOCAL_PASSWORD") ?? string.Empty,
                dbname = Environment.GetEnvironmentVariable("NEO4J_LOCAL_DBNAME") ?? string.Empty
            };
    }

    public Neo4jConfig()
    {
        // uri = !uri.IsNullOrWhiteSpace()
        //     ? uri
        //     : Environment.GetEnvironmentVariable("NEO4J_URI") ?? string.Empty;
        //
        // user = Environment.GetEnvironmentVariable("NEO4J_USER") ?? string.Empty;
        // password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? string.Empty;
        //
        // dbname =
        //     Environment.GetEnvironmentVariable("NEO4J_DBNAME")
        //         // .FallbackOn(
        //         //     Environment.GetEnvironmentVariable("NEO4J_LOCAL_DBNAME")
        //         // ).Value
        //         .FallbackOn("neo4j")
        //         .Value;
        //
        // this.Dump("self");
    }

    public string uri { get; set; } = string.Empty;
    public string user { get; set; }
    public string dbname { get; set; }
    public string password { get; set; }
}


public class Fallback<T>
{
    public static Fallback<T> On(T nextvalue)
    {
        return new Fallback<T>(nextvalue.ToMaybe());
    }

    public static Fallback<T> On(Maybe<T> maybe)
    {
        return new Fallback<T>(maybe);
    }

    private Fallback(Maybe<T> maybe)
    {
        this.maybe = maybe;
    }

    public T Value => maybe.Value;

    public bool is_empty => maybe.IfSome(_ => true);
    private Maybe<T> maybe;
}

public static class FallbackExtensions
{
    public static Fallback<T> FallbackOn<T>(this T next, T fallback_value)
    {
        // if (typeof(T) == typeof(string))
        // {
        //     string maybe_str =(next as string).ToMaybe().Case(some: val => val, none: () => string.Empty);
        //     return Fallback<string>.On("");
        // }

        var maybe = next.ToMaybe().Case(some: val => val, none: () => fallback_value);
        return Fallback<T>.On(maybe);
    }

    public static Fallback<T> Or<T>(this Fallback<T> self, T next)
    {
        return self.is_empty ? Fallback<T>.On(next) : self;
    }
}