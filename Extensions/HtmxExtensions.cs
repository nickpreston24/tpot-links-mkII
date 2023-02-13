using CodeMechanic.Extensions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace CodeMechanic.Advanced.Extensions
{
    /// <summary>
    /// Extensions for working with <see cref="htmx.org"/> responses.
    /// </summary>
    public static class HtmxExtensions
    {

        private static List<string> available_htmx_attributes = new List<string>()
        {
            "hx-get",
            "hx-post",
            "hx-trigger",
            "hx-target",
            "hx-swap",
            "hx-delete",
            "hx-update",
        };

        //public static IEnumerable<AttributeCollection> AreAttributesValid(
        //    this AttributeCollection htmxAttributes
        //    , bool throw_if_malicious = false)
        //{
        //    throw new NotImplementedException();
        //}



        private static readonly IDictionary<Type, ICollection<PropertyInfo>> propertyCache =
          new Dictionary<Type, ICollection<PropertyInfo>>();

        private const string fallback = "div";

        /// <summary>
        /// Surrounds Text with html tag.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string Tag(this string value
            , string tag = fallback
        , string className = ""
        , params string[] attributes
        )
        {
            var attr = !attributes.IsNullOrEmpty() ? attributes.FlattenText() : "";

            string end_tag =
                !tag
                .ToLowerInvariant()
                .Contains("input") ? $"</{tag}>" : $"{tag}>";

            //return new StringBuilder($"<{tag}>{value}")
            //    .Append(end_tag).ToString();

            var html = (!string.IsNullOrWhiteSpace(className))
                   ? $"<{tag} class='{className}' {attr}>{value}{end_tag}"
                   : $"<{tag}{attr}>{value}{end_tag}";

            return html;
        }

        /// <summary>
        /// This is a quick-hack to get things in HTMX format.
        /// Don't use in production, if you know what's good for you.
        /// </summary>
        //[Obsolete(" This is a quick-hack to get things in HTMX format.Don't use in production, if you know what's good for you.")]
        //public static string ToHtmx<T>(this IEnumerable<T> collection
        //   , string outerTag = "ul"
        //   , string innerTag = "li")

        //   where T : class, new()
        //{
        //    return collection
        //        .Select(item => item.ToHtmx(innerTag))
        //        .ToArray()
        //        .FlattenText()
        //        .Tag(outerTag);
        //}

        // public static string ToHtmx(this List<HtmxElement> htmxElements)
        //     => htmxElements
        //     .Select(element => element.ToString())
        //     .FlattenText();


        /// <summary>
        /// This is a quick-hack to get things in HTMX format.
        /// Don't use in production, if you know what's good for you.
        /// </summary>
        [Obsolete(" This is a quick-hack to get things in HTMX format.Don't use in production, if you know what's good for you.")]
        public static string ToHtmx<T>(this T instance,
                                       string innerTag = "li",
                                       string outerTag = "ul",
                                       string className = ""
                                    )
            where T : class, new()
        {
            if (instance == null)
                return string.Empty.Tag(innerTag);

            Type objType = typeof(T);

            // Ensures we always get properties, even if it's an empty set.
            // You can do this with any collection of anything, too
            var properties = propertyCache.TryGetProperties<T>();

            var sb = new StringBuilder();

            properties.Aggregate(sb, (htmxString, nextprop) =>
            {
                //string name = nextprop.Name;
                string raw = nextprop.GetValue(instance)?.ToString().Santize();

                htmxString.Append(raw.Tag(innerTag.Santize(), className.Santize()));

                return htmxString;
            });

            string htmx = sb.ToString()/*.Dump("htmx")*/;

            return htmx;
        }



        // public static HtmxElement Rollup(this IEnumerable<HtmxElement> elements)
        // {
        //     var current = new HtmxElement();

        //     foreach (var element in elements)
        //     {
        //         current = (HtmxElement)(current + element);
        //     }

        //     return current;

        //     //var rolled_up = bogus_htmx_elements.Select((el, index) => new
        //     //{
        //     //    el.DataColumn1,
        //     //    el.DataColumn2,
        //     //    SequenceNo = index + 1
        //     //});
        // }









        public static object ToHTMXResponse<T>(this T instance)
                                                    where T : class, new()
        {
            string html = instance.ToString();

            var response = new HttpResponseMessage
            {
                Content = new StringContent(html)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return response;
        }

        public static HttpResponseMessage ToHTMXResponse<T>(this List<T> items)
            where T : class, new()
        {
            string html = items
                .Aggregate(new StringBuilder(),
                    (text, next) => text.AppendLine(next.ToString()))
                .ToString();

            var response = new HttpResponseMessage
            {
                Content = new StringContent(html)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return response;
        }
    }
}