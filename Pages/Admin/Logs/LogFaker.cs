using Bogus;
using CodeMechanic.Types;
using TPOT_Links;

public class LogFaker : Faker<LogRecord>
{
    private string[] sample_db_names = new[] { "railway", "tpot-links", "tpot-links-db-west", "tpot-links-db-east" };

    private string[] sample_user_names = new[]
        { "Nick Preston", "Braden Preston", "Ronnie Tanner", "Alan Agnew", "Thierry B." };

    public LogFaker()
    {
        RuleFor(o => o.id, f => f.Random.Number(1, 100).ToString());
        RuleFor(o => o.exception_text, f => "Exception: " + f.Lorem.Sentence());
        RuleFor(o => o.created_by, f => sample_user_names.TakeFirstRandom());
        RuleFor(o => o.modified_by, f => sample_user_names.TakeFirstRandom());
        RuleFor(o => o.database_name, f => sample_db_names.TakeFirstRandom());
        RuleFor(o => o.payload, f => f.Random.Number(1, 10).ToString());
    }
}