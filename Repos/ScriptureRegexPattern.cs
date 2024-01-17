using System.Text.RegularExpressions;
using CodeMechanic.Advanced.Extensions;

namespace TPOT_Links.Services;

public class ScriptureRegexPattern : Enumeration
{
    public static readonly IDictionary<ScriptureRegexPattern, System.Text.RegularExpressions.Regex> compiled_patterns =
        new Dictionary<ScriptureRegexPattern, System.Text.RegularExpressions.Regex>();

    public System.Text.RegularExpressions.Regex CompiledPattern { get; }

    public static System.Text.RegularExpressions.Regex GetPattern(ScriptureRegexPattern regexPattern)
    {
        bool is_found = ScriptureRegexPattern.compiled_patterns.TryGetValue(regexPattern, out var found);
        return is_found
            ? found
            : throw new Exception("Pattern with name '" + regexPattern.Name + "' could not be found!");
    }

    public ScriptureRegexPattern(int id
        , string name
        , string pattern
        , RegexOptions options = RegexOptions.None)
        : base(id, name)
    {
        if (options == RegexOptions.None)
            options = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        CompiledPattern =
            new System.Text.RegularExpressions.Regex(pattern, options);

        compiled_patterns.TryAdd(this, this.CompiledPattern);
    }

    public static ScriptureRegexPattern PrefixRegexPattern = new ScriptureRegexPattern(1
        , nameof(PrefixRegexPattern)
        , """(?<Name>[a-zA-Z]+\s+\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]+\r*\n)(?<Text>.*?)(?:\r*\n){2}""");

    public static ScriptureRegexPattern PostFixRegexPattern = new ScriptureRegexPattern(2
        , nameof(PostFixRegexPattern),
        """^(?<Text>.*?)(?<Name>\(\w+\s+\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{2,4}\)\.?)""");

    //https://regex101.com/r/DQu8B1/1
    public static ScriptureRegexPattern FullRegexPattern = new ScriptureRegexPattern(3, nameof(FullRegexPattern),
        """(?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+))*)(?<spaces>(”?$\n)|(”\n*?$)))""");
}