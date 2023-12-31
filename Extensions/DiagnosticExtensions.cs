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

        public static Stopwatch PrintRuntime(this Stopwatch watch)
        {
            var timespan = watch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timespan.Hours, timespan.Minutes, timespan.Seconds,
                timespan.Milliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            return watch;
        }
    }
}