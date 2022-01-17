using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ream.Tools
{
    public static class Extensions
    {
        public static string FirtCharToUpper(this string source)
            => source.Length > 0 ? string.Concat(source[0].ToString().ToUpper(), source.AsSpan(1)) : source;

        public static void WriteLines(this StreamWriter source, string[] lines)
        {
            foreach (string line in lines) source.WriteLine(line);
        }
    }
}
