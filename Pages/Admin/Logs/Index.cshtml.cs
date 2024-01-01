using System.Data;
using System.Diagnostics;
using System.Text;
using CodeMechanic.Async;
using CodeMechanic.Diagnostics;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Types;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using TPOT_Links.Extensions;
using TPOT_Links.Models;
using IEnumerableExtensions = CodeMechanic.Extensions.IEnumerableExtensions;
using StringBuilderExtensions = CodeMechanic.Extensions.StringBuilderExtensions;

namespace TPOT_Links.Pages.Logs
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public string search_term { get; set; } = string.Empty;
        public LogRecord SplunkySearch { get; set; } = new();
        public static List<LogRecord> splunkyLogs { get; set; } = new List<LogRecord>();
        public List<LogRecord> SplunkyLogs => splunkyLogs;

        public void OnGet()
        {
            Console.WriteLine("On get()");
        }

        public async Task<IActionResult> OnGetSeedLogs()
        {
            try
            {
                Console.WriteLine(nameof(OnGetSeedLogs));
                var faker = new LogFaker();
                var sample_records = faker.Generate(1000);
                var insertions = await UpsertLogs(sample_records, debug_mode: false);
                string message = $"Seeded {insertions.Count} logs.";

                return Partial("_Alert", new CustomAlert()
                {
                    Message = message
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.AsCustomAlert(this);
            }
        }

        public async Task<IActionResult> OnPostFullSearchLogs(LogRecord splunky_search)
        {
            Console.WriteLine(nameof(OnPostFullSearchLogs));
            Console.WriteLine("Searching full text for " + search_term);
            var results = RunSearch(search_term, splunky_search, debug_mode: true);
            splunkyLogs = results;
            return Partial("_LogsTable", SplunkyLogs);
        }

        public async Task<IActionResult> OnGetSearchLogs()
        {
            try
            {
                var search = new LogRecord()
                {
                    created_by = "Nick Preston", is_deleted = false, is_archived = false, is_enabled = false
                };

                var results = RunSearch(search_term, search);
                splunkyLogs = results;
                return Partial("_LogsTable", SplunkyLogs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.AsCustomAlert(this);
            }
        }

        private List<LogRecord> RunSearch(
            string search_term
            , LogRecord search_options
            , bool debug_mode = false
        )
        {
            Console.WriteLine(nameof(OnGetSearchLogs));
            // if (debug_mode) search_options.Dump("inputs");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var connectionString = GetConnectionString();
            using var connection = new MySqlConnection(connectionString);

            /* Dapper way */
            var storedProcedureName = "SearchLogs";
            var parameters = new
            {
                search_term = search_term, is_archived = search_options.is_archived,
                is_deleted = search_options.is_deleted, is_enabled = search_options.is_enabled
            };

            var results = connection
                .Query<TPOT_Links.LogRecord>(storedProcedureName
                    , parameters
                    , commandType: CommandType.StoredProcedure
                )
                .ToList();


            watch.Stop();
            watch.PrintRuntime();

            return results;
        }

        private static string? GetConnectionString()
        {
            var connectionString = new MySqlConnectionStringBuilder()
            {
                Database = Environment.GetEnvironmentVariable("MYSQLDATABASE"),
                Server = Environment.GetEnvironmentVariable("MYSQLHOST"),
                Password = Environment.GetEnvironmentVariable("MYSQLPASSWORD"),
                UserID = Environment.GetEnvironmentVariable("MYSQLUSER"),
                Port = 16806
            }.ToString();

            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            return connectionString;
        }

        private async Task<List<LogRecord>> BulkUpsertLogs(List<LogRecord> logRecords, int batch_size = 100)
        {
            var Q = new SerialQueue();
            var tasks = logRecords
                .Batch(batch_size)
                .Select(log_batch => Q
                    .Enqueue(async () => await UpsertLogs(log_batch)));

            await Task.WhenAll(tasks);
            Console.WriteLine($"Done upserting {logRecords.Count} logs!");

            return default;
        }

        private async Task<List<LogRecord>> UpsertLogs(
            IEnumerable<LogRecord> logRecords
            , bool debug_mode = false
        )
        {
            var insert_values = logRecords
                .Aggregate(new StringBuilder(), (builder, next) =>
                {
                    builder.AppendLine($"""
                        ( '{ next.application_name}'
                        , '{ next.database_name}'
                        , '{ next.exception_text}'
                        , '{ next.breadcrumb}'
                        , '{ next.issue_url}' 
                        , '{ next.created_by}'
                        , '{ next.modified_by}'
                        , null
                        , null 
                        , '{ next.is_archived}'
                        , '{ next.is_deleted}'
                        , '{ next.is_enabled}'
                        )
                     """ .Trim());
                    builder.Append(",");
                    return builder;
                }).ToString();

            if (debug_mode) Console.WriteLine("values query :>> " + insert_values);

            string insert_begin = """ 
                        insert into logs 
                        ( 
                            table_name
                         , database_name
                         , exception_text
                         , breadcrumb
                         , issue_url
                         , created_by
                         , modified_by
                         , created_at
                         , modified_at
                         , is_deleted
                         , is_archived
                         , is_enabled
                     )
                    values
                    """;

            var query = StringBuilderExtensions.RemoveFromEnd(new StringBuilder()
                    .AppendLine(insert_begin)
                    .AppendLine(insert_values), 2) // remove last comma
                .Append(";") // adds the delimiter for mysql
                .ToString();

            if (debug_mode) Console.WriteLine("full query :>> " + query);

            try
            {
                var connectionString = GetConnectionString();
                using var connection = new MySqlConnection(connectionString);

                var results = connection
                    .Query<TPOT_Links.LogRecord>(query
                        , commandType: CommandType.Text
                    )
                    .ToList();

                return results ?? new List<LogRecord>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                WriteLocalLogfile(e.ToString() + query);
                throw;
            }
        }

        private static void WriteLocalLogfile(string content)
        {
            var cwd = Environment.CurrentDirectory;
            string filepath = Path.Combine(cwd, "Admin.log");
            Console.WriteLine("writing to :>> " + filepath);
            System.IO.File.WriteAllText(filepath, content);
        }
    }
}


/* Insight way: */
// DbConnection db = new MySqlConnection(connectionString);
// ILogRepository repo = db.As<ILogRepository>();
// Insight.Database.Providers.MsSqlClient.SqlInsightDbProvider.RegisterProvider();
// this.SplunkyLogs = repo.SearchLogs("Nick Preston");


// public interface ILogRepository
// {
//     List<LogRecord> SearchLogs(
//         string created_by
//         , bool? is_archived = false
//         , bool? is_deleted = false
//         , bool? is_enabled = false
//     );
// }