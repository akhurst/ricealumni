using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Contents.Settings;

namespace Downplay.Mechanics
{
    public static class Extensions
    {

        public static void With<T>(this IContent content, Action<T> action) where T : IContent
        {
            T with = content.As<T>();
            if (with != null)
            {
                action.Invoke(with);
            }
        }

        public static void WithPart<T>(this IEnumerable<ContentItem> items, Action<T> action) where T : IContent
        {
            foreach (var content in items.AsPart<T>())
            {
                action.Invoke(content);
            }
        }

        /// <summary>
        /// Check if an item supports versioning
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
/*        public static bool IsDraftable(this IContent content) {
            return (content.Has<IPublishingControlAspect>()
                || content.ContentItem.TypeDefinition.Settings.GetModel<ContentTypeSettings>().Draftable);
        }
        */
        /// <summary>
        /// Parse string to an Int32 value. Returns null if unable to parse.
        /// Slightly friendlier than Int32.Parse or even Int32.TryParse!
        /// </summary>
        /// <seealso cref="ParseLong"/>
        /// <param name="s">The input string to parse</param>
        /// <returns>Null if parse fails, others nullable int of parsed value</returns>
        public static Int32? ParseInt(this String s)
        {
            int result;
            if (Int32.TryParse(s, out result))
            {
                return result;
            }
            return null;
        }
        /// <summary>
        /// Parse string to an Int64 value. Returns null if unable to parse.
        /// Slightly friendlier than Int64.Parse and especially more than Int64.TryParse!
        /// </summary>
        /// <seealso cref="ParseInt"/>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Int64? ParseLong(this String s)
        {
            Int64 result;
            if (Int64.TryParse(s, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Parse a whole list of ints
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<Int32> ParseInt(this IEnumerable<string> list) {
            foreach (var s in list) {
                var parse = s.ParseInt();
                if (parse.HasValue) yield return parse.Value;
            }
        }


        /// <summary>
        /// Parse a whole list of longs
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<Int64> ParseLong(this IEnumerable<string> list) {
            foreach (var s in list) {
                var parse = s.ParseLong();
                if (parse.HasValue) yield return parse.Value;
            }
        }

        /// <summary>
        /// Glues a number of string parts together with a separator
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static string Glue(this IEnumerable<String> parts, string glue)
        {
            return String.Join(glue, parts.ToArray());
        }

        /// <summary>
        /// Checks for a k/v pair in a dictionary avoiding null errors etc.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has(this IDictionary<String, String> dict, string key, string value)
        {
            return dict.ContainsKey(key) && dict[key] == value;
        }

        public static string ConcatPaths(this IEnumerable<String> paths, string separator="/")
        {
            // Eliminate empty parts and glue them together
            return paths.Where(p => p != null && !String.IsNullOrWhiteSpace(p)).Select(p => p.Trim()).Glue(separator);
        }

    }
}