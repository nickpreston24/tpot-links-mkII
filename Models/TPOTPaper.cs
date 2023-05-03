namespace TPOT_Links;


public class TPOTPaper
{
    public int id { get; set; } = -9999;
    public string Markdown { get; set; } = string.Empty;
    public string FrontMatter { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string RawJson { get; set; } = string.Empty;
    public int Author { get; set; } = -1;
    public string AuthorName { get; set; } = string.Empty;

}

 /*
    id int,
    wordpress_id int,
    author varchar(255),
    category varchar(255),
    link varchar(255),
    excerpt TEXT,
    markdown TEXT,
    frontmatter TEXT,
    RawJson JSON
    */