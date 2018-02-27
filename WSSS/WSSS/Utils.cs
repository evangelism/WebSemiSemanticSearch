using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WSSS
{
    public static class Utils
    {
        public static T[] AsArray<T>(this T x)
        {
            return new T[] { x };
        }

        public static bool PropertyExists(JObject obj, string name)
        {
            return obj.Property(name)!=null;
        }

        public static string ApplyRegex(this string inp, string RegEx, Func<string, string> f)
        {
            var rx = new Regex(RegEx);
            return rx.Replace(inp, (Match m) => f(m.ToString()));
        }

        public static string BuildDesc(JObject o, string[] props)
        {
            var sb = new StringBuilder();
            foreach(var x in props)
            {
                if (o.Property(x)!=null)
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.AppendLine($"{x}: {o.Property(x).Value}");
                }
            }
            return sb.ToString();
        }
    }
}