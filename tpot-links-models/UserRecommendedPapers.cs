namespace TPOT_Links.Models;

public class UserRecommendedPapers
{
    public int length { get; set; } = 0;
    public string[] keys { get; set; }

    public _fields[] _fields { get; set; }
    public _fieldLookup _fieldLookup { get; set; }
}

public record _fields(
    Identity identity,
    string[] labels,
    Properties properties,
    string elementId,
    Start start,
    End end,
    string type,
    string startNodeElementId,
    string endNodeElementId,
    int low,
    int high
);

public record Identity(
    int low,
    int high
);

public record Properties(
    string last_name,
    string first_name,
    string name,
    string Title,
    Id id
);

public record Id(
    int low,
    int high
);

public record Start(
    int low,
    int high
);

public record End(
    int low,
    int high
);

public record _fieldLookup(
    int user,
    int likes,
    int paper,
    int occurrence
);