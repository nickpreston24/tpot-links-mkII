using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using CodeMechanic.Advanced.Regex;
using CodeMechanic.Async;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.FileSystem;
using CodeMechanic.Neo4j.Repos;
using CodeMechanic.PuppeteerExtensions;
using CodeMechanic.Reflection;
using CodeMechanic.Types;
using Htmx;
using Insight.Database;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using NSpecifications;
using PuppeteerSharp;
using TPOT_Links;
using TPOT_Links.Pages.Sandbox;

namespace tpot_links_seeder.Controllers;
[Produces("application/json")]
[Route("[controller]")]
public class TPOTPaperController : ControllerBase
{
    private static readonly IDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
        new Dictionary<Type, ICollection<PropertyInfo>>();

    private readonly ILogger<TPOTPaperController> logger;
    private readonly IWebHostEnvironment env;
    private readonly TPOTSettings settings;
    private readonly INeo4JRepo neo4JRepo;
    private readonly IEmbeddedResourceQuery embeddedResources;

    public TPOTPaperController(
        ILogger<TPOTPaperController> logs
        , INeo4JRepo neo4j_repo
        , IWebHostEnvironment environment_vars
        , IEmbeddedResourceQuery embeddedResources
    )
    {
        neo4JRepo = neo4j_repo;
        this.embeddedResources = embeddedResources;
        logger = logs;
        env = environment_vars;

        settings = new TPOTSettings()
            .With(setting =>
            {
                setting.Neo4jUri = "blarg";
                setting.Neo4jUser = "blarg";
                setting.Neo4jPassword = "blarg";
                setting.MySqlConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTIONSTRING");
            })
            .Dump("current settings"); // doesn't work on startup.  Who knew?
    }

    // GET: api/Paper
    [HttpGet]
    public IEnumerable<Paper> Get()
    {
        return new Paper()
        {
            Title = "Test",
            Description = "Your api is working."
        }.AsList();
    }

    [HttpPost(nameof(SearchByRegex))]
    public async Task<IActionResult> SearchByRegex([FromBody] PaperSearch search_parameters)
    {
        /**
         * Idea:
         * What if I can just pass the file name and/or file ext, "SearchByRegex.cypher" and run it?
         * Then, cast the result to a passed in Type (like Paper)
         *
         * Pros - This could eliminate the need for many controller endpoints
         * Cons - This could be very generic, at least at first.
         */
        
        string expected_name = "SearchByRegex.cypher";
        string query = await embeddedResources
            .GetQueryAsync<IndexModel>(new StackTrace());
        
        if (string.IsNullOrWhiteSpace(query))
        {
            string cwd = Directory.GetCurrentDirectory();
            cwd.Dump("current dir");

            var fs = new Grepper()
            {
                RootPath = cwd,
                FileSearchMask = "*.cypher"
            };

            string found_file = fs.GetFileNames()
                .Dump("all found")
                .FirstOrDefault(filename => filename.Contains(expected_name, StringComparison.OrdinalIgnoreCase));
            found_file.Dump("file found ");
            query = System.IO.File.ReadAllText(found_file);
        }

        var papers = await neo4JRepo.SearchNeo4J<Paper>(query, search_parameters);

        return Request.IsHtmx()
            ? Content("<h2>Hello, From Named Route</h2>")
            : Ok(papers);
    }

    [HttpPost(nameof(SaveLog))]
    public async Task<string> SaveLog([FromBody] LogRecord log_record)
    {
        return string.Empty;
    }

    [HttpGet(nameof(GetCommentsFromLocalHtml))]
    public async Task<List<FacebookComment>> GetCommentsFromLocalHtml()
    {
        string root_folder = Path.Combine(env.ContentRootPath
                , "Samples")
            .Dump("root");

        var queue = new SerialQueue();
        var tasks = new List<Task<TPOTMarkdownPaper>>();

        var grepper = new Grepper()
            .With(grep =>
            {
                grep.RootPath = root_folder;
                grep.FileNamePattern = @".*\.html";
            });

        var all_files = grepper
            .GetFileNames()
            .Dump("Files");

        var first_file = all_files.Last();
        string text = System.IO.File.ReadAllText(first_file);

        text.Length.Dump("length of file");

        var encoded_comments = text
            .Extract<FacebookComment>(RegexPatterns.FacebookComments.First().Value);

        encoded_comments.Dump("encoded comment classes");

        // var all_papers = all_files
        //         .Select(file_path => new TPOTPaper()
        //             .With(p =>
        //             {
        //                 p.FilePath = file_path;
        //                 p.RawText = System.IO.File.ReadAllText(file_path)
        //                     .Trim();
        //             }))
        //         .Take(limit)
        //     ;

        return encoded_comments;
    }

    [HttpGet(nameof(CreatePapersFromLocalJson))]
    public async Task<List<Paper>> CreatePapersFromLocalJson(
        int limit = 1
        , string json_root_folder
            = "tpot_static_wip/cache"
    )
    {
        string root_folder = Path
                .Combine(env.ContentRootPath.GoUp(), json_root_folder)
                .Dump("starting at root folder")
            ;

        var watch = new Stopwatch();
        watch.Start();

        var queue = new SerialQueue();
        var tasks = new List<Task<TPOTMarkdownPaper>>();

        var grepper = new Grepper()
            .With(grep =>
            {
                grep.RootPath = root_folder;
                grep.FileNamePattern = @".*\.json";
            });

        var all_files = grepper.GetFileNames() /*.Dump("Files")*/;

        var all_papers = all_files
                .Select(file_path => new TPOTJsonPaper()
                    .With(json_file =>
                    {
                        json_file.FilePath = file_path;
                        json_file.RawText = System.IO.File.ReadAllText(file_path)
                            .Trim();
                    }))
                .Take(limit)
                .ToList()
            ;

        // var paper_properties = _propertyCache
        //     .TryGetProperties<Paper>(true)
        //     .ToArray();

        var wordpressPapers = all_papers
            .Take(limit)
            .Aggregate(
                new List<WordpressPaper>(),
                (papers, next_paper) =>
                {
                    string json = next_paper.RawText;
                    // Console.WriteLine(json);

                    // TODO: Make this conditional between an array of objects and a single object.
                    // var deserialized_set_of_papers = JsonConvert.DeserializeObject<List<Paper>>(json);
                    // papers.AddRange(deserialized_set_of_papers);

                    var single_paper_deserialized = JsonConvert.DeserializeObject<WordpressPaper>(json);
                    papers.Add(single_paper_deserialized);
                    return papers;
                }
            );

        Console.WriteLine($"Uploading {wordpressPapers.Count} raw papers to Neo4j....");

        var neo_papers = wordpressPapers
            .Select(wp_paper => wp_paper
                .Map(wordpress => new Paper
                {
                    Content = wordpress?.content?.rendered, Title = wordpress?.title?.rendered,
                    Excerpt = wordpress?.excerpt?.rendered,
                    Categories = string.Join(",", wordpress.categories), Slug = wordpress?.slug, Type = wordpress?.type,
                    Url = wordpress?.link,
                    AuthorId = wordpress.author, Id = wordpress.id.ToString(), last_modified_at_wp = wordpress.modified,
                    created_at_wp = wordpress.date, created = DateTime.Now.ToString()
                }));

        neo_papers.Count().Dump("all neo papers");
        var parameters = new
        {
            batch = neo_papers
        };

        // https://stackoverflow.com/questions/69200606/merge-with-unwind-issue-neo4j
        string query = """
                           WITH $batch AS batch
                           UNWIND batch as ind
                           MERGE (n:Paper{Title: ind.Title})
                           SET n += ind
                       """;

        var created = await neo4JRepo.BulkCreateNodes<Paper>(query, parameters);

        return created.ToList();
    }

    private static void TestSingleFile(List<TPOTJsonPaper> all_papers)
    {
        string test_json = all_papers.FirstOrDefault()?.RawText;

        all_papers.FirstOrDefault().FilePath.Dump("First file");

        var test = all_papers.FirstOrDefault().RawText;
        var res = JsonConvert.DeserializeObject<WordpressPaper>(test);
        res.Dump("deserialized");

        Console.WriteLine(test_json);
    }

    [HttpGet(nameof(CreatePapersFromLocalMarkdown))]
    public async Task<TPOTPapersResult> CreatePapersFromLocalMarkdown(int limit = 15)
    {
        // var options = RegexOptions.Compiled
        //             | RegexOptions.IgnoreCase
        //             | RegexOptions.ExplicitCapture
        //             | RegexOptions.Multiline
        //             | RegexOptions.IgnorePatternWhitespace;

        string markdown_extraction_pattern = RegexPatterns.MarkdownExtractor.Last().Value;
        string hugo_paper_pattern = RegexPatterns.Hugos.Last().Value;
        string frontmatter_pair_pattern = RegexPatterns.FrontMatter.Last().Value;

        string root_folder = Path.Combine(env.ContentRootPath
                .GoUp(), "tpot_static_wip")
            .Dump("root");

        var watch = new Stopwatch();
        watch.Start();

        var queue = new SerialQueue();
        var tasks = new List<Task<TPOTMarkdownPaper>>();

        var grepper = new Grepper()
            .With(grep =>
            {
                grep.RootPath = root_folder;
                grep.FileNamePattern = @".*\.md";
            });

        var all_files = grepper.GetFileNames() /*.Dump("Files")*/;

        var all_papers = all_files
                .Select(file_path => new TPOTMarkdownPaper()
                    .With(p =>
                    {
                        p.FilePath = file_path;
                        p.RawText = System.IO.File.ReadAllText(file_path)
                            .Trim();
                    }))
                .Take(limit)
            ;

        var paper_properties = _propertyCache
            .TryGetProperties<TPOTMarkdownPaper>(true)
            .ToArray();

        foreach (var paper in all_papers)
        {
            tasks.Add(queue.Enqueue(async () =>
            {
                HugoPaper hugo_paper = paper.RawText
                        .Extract<HugoPaper>(hugo_paper_pattern)
                        .FirstOrDefault()
                        .ToMaybe()
                        .Case(
                            some: hugo =>
                                hugo.With(h =>
                                {
                                    // This fixes the pesky escaped newline literals.
                                    h.FrontMatter = Regex.Replace(h.FrontMatter, @"\r\n?|\n", Environment.NewLine)
                                        .Trim();
                                    paper.Markdown = string.IsNullOrWhiteSpace(h.RawMarkdown)
                                        ? h.RawMarkdown
                                        : string.Empty;
                                }),
                            none: () => new HugoPaper()
                        )
                    ;

                paper.FrontMatter = hugo_paper?.FrontMatter?.Trim();

                paper.Markdown = !string.IsNullOrWhiteSpace(hugo_paper?.RawMarkdown)
                    ? hugo_paper?.RawMarkdown
                    : "";
                var pairs =
                        string.IsNullOrWhiteSpace(paper.FrontMatter)
                            ? new Dictionary<string, string>()
                            : paper.FrontMatter
                                // .Dump("raw frontmatter")
                                .Extract<FrontmatterPair>(frontmatter_pair_pattern)
                                .Dump("PAIRS")
                                .ToDictionary(
                                    p => p.Label,
                                    p => p.Value
                                )
                    ;

                paper.FrontmatterPairs = pairs;

                var matching_props = paper_properties
                    .Where(p => pairs.Keys //.Dump("keys")
                        .Contains(p.Name, StringComparer.OrdinalIgnoreCase));

                foreach (var prop in matching_props)
                {
                    Type prop_type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    string name = prop.Name.ToLower();

                    object safe_value =
                        pairs[name] == null
                            ? null
                            : Convert.ChangeType(pairs[name], prop_type);

                    prop.SetValue(paper, safe_value, null);
                }

                return paper;
            }));
        }

        var results = (await Task.WhenAll(tasks)).ToList();
        // results.Dump("RESULTS");

        var valid = new Spec<TPOTMarkdownPaper>(paper =>
                paper.id > 0
            // && paper.Markdown.Length > 0
        );

        var invalid = !valid;

        var passing = results
                .Where(valid)
                .ToList()
            // .Dump("passing")
            ;

        var non_passing = results
            .Where(invalid)
            .ToList();

        non_passing
            .FirstOrDefault()
            .Dump("bad egg");

        var response = new TPOTPapersResult()
            .With(res =>
            {
                res.Papers = results;
                res.Elapsed = watch.Elapsed.ToString();
                res.Count = results.Count;
                res.valid_papers = passing.Count;
                res.invalid_papers = non_passing.Count;
                res.total_files_on_disk = all_files.ToList().Count;
            });

        return response /*.Dump("RESPONSE")*/;
    }

    [HttpPost(nameof(StoreNewPaper))]
    public async Task<IEnumerable<TPOTMarkdownPaper>> StoreNewPaper([FromBody] TPOTMarkdownPaper incomingMarkdownPaper)
    {
        settings.Dump("current settings");

        string connection_string = settings.MySqlConnectionString;

        using (MySqlConnection connection = new MySqlConnection(connection_string))
        {
            string query = """select * from railway.TPOTPapers;""";

            var results = connection
                .QuerySql<TPOTMarkdownPaper>(query)
                .ToMaybe(); // I like maybe.  So sue me.

            return results
                .Case(
                    some: (papers) => papers?.Dump("results"),
                    none: () =>
                    {
                        "No results.  Sending back original paper".Dump();
                        return incomingMarkdownPaper.AsList();
                    }
                );
        }
    }

   

    [HttpGet(nameof(TakeScreenshot))]
    public async Task<FacebookPost> TakeScreenshot(
        // string url = "http://www.google.com"
        string url =
            "https://www.facebook.com/officialbenshapiro/posts/pfbid0235H6HMsAdpGqULNzW4okjNxc5M31Fr6oof51GusMhMEtHq5tGMGoYdamG1JtHgbwl?comment_id=187601347521345&reply_comment_id=153781794119258&notif_id=1683167179141365&notif_t=comment_mention&ref=notif"
        , string save_folder = "screenshots")
    {
        string output_folder = $"./{save_folder}";
        if (!Directory.Exists(output_folder))
            Directory.CreateDirectory(output_folder);

        var post = FacebookPost.CreateInstance(url)
            .Dump("post");

        string outfile_path = $"screenshots/{post.OutputPath}";
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
        var page = await browser.NewPageAsync();
        await page.GoToAsync(url);
        await page.ScreenshotAsync(outfile_path);


        return post;
    }


    [HttpGet(nameof(SaveFullPageAsPDF))]
    public async Task<FacebookPost> SaveFullPageAsPDF(
        // string url = "http://www.google.com"
        string url =
            "https://www.facebook.com/officialbenshapiro/posts/pfbid0235H6HMsAdpGqULNzW4okjNxc5M31Fr6oof51GusMhMEtHq5tGMGoYdamG1JtHgbwl?comment_id=187601347521345&reply_comment_id=153781794119258&notif_id=1683167179141365&notif_t=comment_mention&ref=notif"
        , string save_folder = "pdf")
    {
        string output_folder = $"./{save_folder}";
        if (!Directory.Exists(output_folder))
            Directory.CreateDirectory(output_folder);

        var post = FacebookPost.CreateInstance(url)
            .Dump("post")
            .With(p => p.Extension = ".pdf");

        string output_file_path = $"screenshots/{post.OutputPath}".Dump("saving as:");
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
        var page = await browser.NewPageAsync();
        await page.GoToAsync("http://www.google.com");
        await page.PdfAsync(output_file_path);

        return post;
    }

    [HttpGet(nameof(SaveFullPageAsHTML))]
    public async Task<FacebookPost> SaveFullPageAsHTML(
        // string url = "http://www.google.com"
        string url =
            "https://www.facebook.com/officialbenshapiro/posts/pfbid0235H6HMsAdpGqULNzW4okjNxc5M31Fr6oof51GusMhMEtHq5tGMGoYdamG1JtHgbwl?comment_id=187601347521345&reply_comment_id=153781794119258&notif_id=1683167179141365&notif_t=comment_mention&ref=notif"
        // https://www.facebook.com/officialbenshapiro/posts/pfbid0235H6HMsAdpGqULNzW4okjNxc5M31Fr6oof51GusMhMEtHq5tGMGoYdamG1JtHgbwl?comment_id=187601347521345&reply_comment_id=153781794119258&notif_id=1683167179141365&notif_t=comment_mention&ref=notif
        , string save_folder = "html")
    {
        string output_folder = $"./{save_folder}";
        if (!Directory.Exists(output_folder))
            Directory.CreateDirectory(output_folder);

        var post = FacebookPost.CreateInstance(url)
            .Dump("post")
            .With(p => p.Extension = ".html");

        string output_file_path = $"screenshots/{post.OutputPath}".Dump("saving as:");
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var options = new LaunchOptions
        {
            Headless = true
        };

        using (var browser = await Puppeteer.LaunchAsync(options))
        using (var page = await browser.NewPageAsync())
        {
            string div_contents = (await page.GetHtmlPropertyFromElement(element: "blah")).ToString();

            var all_encoded_comment_classes = div_contents
                .Extract<FacebookComment>(RegexPatterns.FacebookComments.First().Value);

            all_encoded_comment_classes.Dump("encoded comment selectors found");

            // var parsed_comment_selector = ".devsite-suggest-all-results";
            // await page.WaitForSelectorAsync(parsed_comment_selector);
            // await page.ClickAsync(parsed_comment_selector);

            // continue the operation
        }

        return post;
    }
}