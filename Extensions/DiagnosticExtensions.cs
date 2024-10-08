﻿// using System.Diagnostics;
// using CodeMechanic.Diagnostics;
//
// namespace TPOT_Links.Extensions
// {
//     public static class DiagnosticExtensions
//     {
//         public static bool OverridesToString(this object v)
//         {
//             return v.ToString() != v.GetType().ToString();
//         }
//
//         public static void Print(params object[] items)
//         {
//             foreach (var item in items)
//             {
//                 if (item.OverridesToString())
//                     Console.WriteLine(item);
//                 else
//                     item.Dump();
//             }
//         }
//
//         public static T QuickWatch<T>(this Func<T> fn, string message = "quick watch :>> ")
//         {
//             Stopwatch watch = new Stopwatch();
//             watch.Start();
//             var result = fn();
//             watch.Stop();
//             Console.WriteLine(message + watch.Elapsed);
//             return result;
//         }
//
//         public static Stopwatch PrintRuntime(this Stopwatch watch, string message = "RunTime: ")
//         {
//             var timespan = watch.Elapsed;
//             string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
//                 timespan.Hours, timespan.Minutes, timespan.Seconds,
//                 timespan.Milliseconds);
//             Console.WriteLine(message + elapsedTime);
//             return watch;
//         }
//     }
// }