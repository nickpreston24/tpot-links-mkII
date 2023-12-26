using System.Data;
using System.Data.Common;
using CodeMechanic.Diagnostics;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using TPOT_Links;

namespace TPOT_Links.Pages.Logs
{
    public class IndexModel : PageModel
    {
        public List<LogRecord> SplunkyLogs { get; set; } = new List<LogRecord>();

        // public void OnGet()
        // {
        //     Console.WriteLine("hello there!");
        // }

        public async Task<IActionResult> OnGetSearchLogs()
        {
            Console.WriteLine("Searching logs...");
            // var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTIONSTRING");
            // var connectionString = Environment.GetEnvironmentVariable("MYSQL_PRIVATE_URL");
            var connectionString = new MySqlConnectionStringBuilder()
            {
                Database = Environment.GetEnvironmentVariable("MYSQLDATABASE"),
                Server = Environment.GetEnvironmentVariable("MYSQLHOST"),
                Password = Environment.GetEnvironmentVariable("MYSQLPASSWORD"),
                UserID = Environment.GetEnvironmentVariable("MYSQLUSER"),
                Port = 16806 //Environment.GetEnvironmentVariable("MYSQLPORT").ToUInt(),
                // ConnectionTimeout = 60
            }.ToString();

            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            connectionString.Dump("mysql connectionstrng");
            using var connection = new MySqlConnection(connectionString);

            /* Dapper way */
            var storedProcedureName = "SearchLogs";
            var values = new
            {
                created_by = "Nick Preston", is_deleted = false, is_archived = false, is_enabled = false
            };
            var results = connection
                .Query<TPOT_Links.LogRecord>(storedProcedureName, values, commandType: CommandType.StoredProcedure)
                .ToList();

            results.Dump("splunky logs");

            /* Insight way: */
            // DbConnection db = new MySqlConnection(connectionString);
            // ILogRepository repo = db.As<ILogRepository>();
            // Insight.Database.Providers.MsSqlClient.SqlInsightDbProvider.RegisterProvider();
            // this.SplunkyLogs = repo.SearchLogs("Nick Preston");


            SplunkyLogs = results;

            return Partial("_LogsTable", SplunkyLogs);
        }
    }
}

public interface ILogRepository
{
    List<LogRecord> SearchLogs(
        string created_by
        , bool? is_archived = false
        , bool? is_deleted = false
        , bool? is_enabled = false
    );
}