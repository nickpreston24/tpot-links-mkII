using CodeMechanic.Advanced.Regex;
using TPOT_Links.Models;

namespace TPOT_Links.Services;

public class ScriptureParser : IParseScriptures
{
    public List<Scripture> ParseLines(ScriptureRegexPattern pattern, string lines)
    {
        return lines.Extract<Scripture>();
    }

    public List<Scripture> ParseLines(
        ScriptureRegexPattern regexPattern
        , params string[] lines)
    {
        var scriptures = lines
            .SelectMany(line => line
                .Extract<Scripture>(ScriptureRegexPattern.GetPattern(regexPattern)))
            .ToList();

        return scriptures;
    }
}

// best so far:

// (?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+(?<return>”?$\n\s*){1,}))*))

//(?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+))*)(?<return>(”?$\n)|(”\n*?$)))

// (?<return>”\n*?$))