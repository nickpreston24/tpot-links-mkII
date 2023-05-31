using CodeMechanic.Advanced.Regex;

namespace TPOT_Links.Models;

public class ScriptureParser
{
  private List<Scripture> Prefixed { get; set; } = new List<Scripture>();
  private List<Scripture> Postfixed { get; set; } = new List<Scripture>();
  private List<Scripture> Full { get; set; } = new List<Scripture>();
  
  // best:

  // (?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+(?<return>”?$\n\s*){1,}))*))
  
  //(?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+))*)(?<return>(”?$\n)|(”\n*?$)))

  // (?<return>”\n*?$))


  private const string PrefixPattern = """(?<Name>[a-zA-Z]+\s+\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]+\r*\n)(?<Text>.*?)(?:\r*\n){2}""";

  private const string PostFixPattern = """^(?<Text>.*?)(?<Name>\(\w+\s+\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{2,4}\)\.?)""";

  //https://regex101.com/r/DQu8B1/1
  private const string FullPattern = """(?(?=^\s*\((?<chapter>^\d*\s*[a-zA-Z]+\s*\d{1,3}:?\d{1,2}-?\d{1,2}\s+[A-Z]{3,})\)\.)$)(“(?<quoted_text>\b[\s\.,a-zA-Z!:\d]+\b)”\s*?(?<end>\([\w\s:-]+\)\.))|((?<start>^\d*?\s*?[A-Z][\w\s:-]+$)\n(?<quote>^((\s*“?\(\d+\)[\w\s,\.!?’',”;:]+))*)(?<spaces>(”?$\n)|(”\n*?$)))""";

  public ScriptureParser(params string [] lines) {

    Full = lines
      .SelectMany(line => line
      .Extract<Scripture>(FullPattern))
      .ToList();

    // Prefixed = lines
    //   .SelectMany(line => line
    //   .Extract<Scripture>(PrefixPattern))
    //   .ToList();

    // Postfixed = lines
    //   .SelectMany(line => line
    //   .Extract<Scripture>(PostFixPattern))
    //   .ToList();
  }

  public void Deconstruct(out List<Scripture> prefixed
  , out List<Scripture> postfixed
  , out List<Scripture> full)
  {
      prefixed = Prefixed;
      postfixed = Postfixed;
      full = Full;
  }  
}