using System.Data;
using System.Diagnostics;
using System.Text;
using CodeMechanic.Async;
using CodeMechanic.Types;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using TPOT_Links.Extensions;
using TPOT_Links.Models;
using StringBuilderExtensions = CodeMechanic.Extensions.StringBuilderExtensions;

namespace TPOT_Links.Pages.Logs
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public string search_term { get; set; } = string.Empty;
        public LogRecord SplunkySearch { get; set; } = new();
        public static List<LogRecord> splunkyLogs { get; set; } = new List<LogRecord>();
        public List<LogRecord> SplunkyLogs => splunkyLogs;

        public void OnGet()
        {
            Console.WriteLine("On get()");
        }

        public async Task<IActionResult> OnGetCancel()
        {
            Console.WriteLine("Cancelling current task ... ");
            cancellationTokenSource.Cancel();
            return Partial("_CancelButton", new CancelButtonOptions() { Message = "Cancelled " });
        }

        public async Task<IActionResult> OnGetSeedLogs()
        {
            try
            {
                Console.WriteLine(nameof(OnGetSeedLogs));
                var faker = new LogFaker();
                var sample_records = faker.Generate(1000);
                Console.WriteLine("sample records :>> " + sample_records.Count);
                cancellationTokenSource = new CancellationTokenSource();
                var insertions = await BulkUpsertLogs(sample_records)
                        .WithCancellation(cancellationTokenSource.Token)
                    ;
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
            cancellationTokenSource = new CancellationTokenSource();
            Console.WriteLine(nameof(OnPostFullSearchLogs));
            Console.WriteLine("Searching full text for " + search_term);
            var results = RunSearchAsync(search_term, splunky_search, debug_mode: true)
                .WithCancellation(cancellationTokenSource.Token);
            splunkyLogs = await results;
            return Partial("_LogsTable", SplunkyLogs);
        }

        public async Task<IActionResult> OnGetSearchLogs()
        {
            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                var search = new LogRecord()
                {
                    created_by = "Nick Preston", is_deleted = false, is_archived = false, is_enabled = false
                };

                var searchTask = RunSearchAsync(search_term, search)
                        .WithCancellation(cancellationTokenSource.Token)
                    ;
                splunkyLogs = await searchTask;
                return Partial("_LogsTable", SplunkyLogs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.AsCustomAlert(this);
            }
        }

        private async Task<List<LogRecord>> RunSearchAsync(
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
            await using var connection = new MySqlConnection(connectionString);

            /* Dapper way */
            var storedProcedureName = "SearchLogs";
            var parameters = new
            {
                search_term = search_term, is_archived = search_options.is_archived,
                is_deleted = search_options.is_deleted, is_enabled = search_options.is_enabled
            };

            var results = (await connection
                    .QueryAsync<TPOT_Links.LogRecord>(storedProcedureName
                        , parameters
                        , commandType: CommandType.StoredProcedure
                    )
                    .WithCancellation(cancellationTokenSource.Token))
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

        private async Task<List<LogRecord>> BulkUpsertLogs(List<LogRecord> logRecords)
        {
            var batch_size =
                1000; //(int)Math.Round(Math.Log2(logRecords.Count * 1.0) * Math.Log10(logRecords.Count) * 100, 1);
            Console.WriteLine("batch size :>> " + batch_size);
            var Q = new SerialQueue();
            Console.WriteLine("Running Q of bulk upserts ... ");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var tasks = logRecords
                .Batch(batch_size)
                .Select(log_batch => Q
                    .Enqueue(async () => await UpsertLogs(log_batch, debug_mode: false)));

            await Task.WhenAll(tasks);
            watch.Stop();
            watch.PrintRuntime($"Done upserting {logRecords.Count} logs! ");
            return logRecords;
        }

        private async Task<List<LogRecord>> UpsertLogs(
            IEnumerable<LogRecord> logRecords
            , bool debug_mode = false
        )
        {
            var insert_values = logRecords
                .Aggregate(new StringBuilder(), (builder, next) =>
                {
                    builder
                        .AppendLine($"( '{next.application_name}'")
                        .AppendLine($", '{next.database_name}'   ")
                        .AppendLine($", '{next.exception_text}'  ")
                        .AppendLine($", '{next.breadcrumb}'      ")
                        .AppendLine($", '{next.issue_url}'       ")
                        .AppendLine($", '{next.created_by}'      ")
                        .AppendLine($", '{next.modified_by}'     ")
                        .AppendLine($", null")
                        .AppendLine($", null")
                        .AppendLine($", '{next.is_archived}'     ")
                        .AppendLine($", '{next.is_deleted}'      ")
                        .AppendLine($", '{next.is_enabled}'      ")
                        .AppendLine($")")
                        .ToString()
                        .Trim();
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