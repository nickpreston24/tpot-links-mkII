using System.Diagnostics;

namespace TPOT_Links.Extensions
{
    public static class DiagnosticExtensions
    {
        public static T QuickWatch<T>(this Func<T> fn, string message = "quick watch :>> ")
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var result = fn();
            watch.Stop();
            Console.WriteLine(message + watch.Elapsed);
            return result;
        }
    }
}