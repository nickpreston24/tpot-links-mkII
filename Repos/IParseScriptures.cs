using TPOT_Links.Models;

namespace TPOT_Links.Services;

public interface IParseScriptures
{
    List<Scripture> ParseLines(ScriptureRegexPattern regexPattern
        , params string[] lines);

    List<Scripture> ParseLines(ScriptureRegexPattern regexPattern
        , string lines);
}