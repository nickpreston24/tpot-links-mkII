// using CodeMechanic.Types;
//
// namespace CodeMechanic.RazorHAT.Models;
//
// public class RazorPageRoute
// {
//     public string url { get; set; } = string.Empty;
//     public string parent { get; set; } = string.Empty;
//     public string breadcrumb { get; set; } = string.Empty;
//     public string text { get; set; } = string.Empty;
//     public bool enabled { get; set; }
//     public bool external { get; set; }
//
//     public RazorPageRoute(BreadCrumbParts parts)
//     {
//         string value = parts
//             .ToMaybe()
//             .IfSome(crumb => !string.IsNullOrWhiteSpace(crumb.PageName)
//                 ? crumb.PageName
//                 : "???");
//
//         text = value;
//         parent = parts?.Parent;
//     }
// }