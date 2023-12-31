using System.Data;
using System.Diagnostics;
using CodeMechanic.Diagnostics;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using TPOT_Links.Extensions;

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

        public void OnGetSeedLogs()
        {
            
        }

        public async Task<IActionResult> OnPostFullSearchLogs(LogRecord splunkySearch)
        {
            var results = RunSearch(splunkySearch.Dump("inputs"));
            splunkyLogs = results;
            return Partial("_LogsTable", SplunkyLogs);
        }

        public async Task<IActionResult> OnGetSearchLogs()
        {
            var search = new LogRecord()
            {
                created_by = "Nick Preston", is_deleted = false, is_archived = false, is_enabled = false
            };

            var results = RunSearch(search);
            splunkyLogs = results;
            return Partial("_LogsTable", SplunkyLogs);
        }

        private List<LogRecord> RunSearch(LogRecord search)
        {
            Console.WriteLine(nameof(OnGetSearchLogs));
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var connectionString = new MySqlConnectionStringBuilder()
            {
                Database = Environment.GetEnvironmentVariable("MYSQLDATABASE"),
                Server = Environment.GetEnvironmentVariable("MYSQLHOST"),
                Password = Environment.GetEnvironmentVariable("MYSQLPASSWORD"),
                UserID = Environment.GetEnvironmentVariable("MYSQLUSER"),
                Port = 16806
            }.ToString();

            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            using var connection = new MySqlConnection(connectionString);

            /* Dapper way */
            var storedProcedureName = "SearchLogs";
            var results = connection
                .Query<TPOT_Links.LogRecord>(storedProcedureName, search, commandType: CommandType.StoredProcedure)
                .ToList();

            /* Insight way: */
            // DbConnection db = new MySqlConnection(connectionString);
            // ILogRepository repo = db.As<ILogRepository>();
            // Insight.Database.Providers.MsSqlClient.SqlInsightDbProvider.RegisterProvider();
            // this.SplunkyLogs = repo.SearchLogs("Nick Preston");

            watch.Stop();
            watch.PrintRuntime();

            return results;
        }
    }
}

// public interface ILogRepository
// {
//     List<LogRecord> SearchLogs(
//         string created_by
//         , bool? is_archived = false
//         , bool? is_deleted = false
//         , bool? is_enabled = false
//     );
// }