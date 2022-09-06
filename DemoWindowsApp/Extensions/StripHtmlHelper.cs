using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoWindowsApp.Extensions
{
    public static class StripHtmlHelper
    {
        public static string StripHtmlTags(this string source)
        {
            return Regex.Replace(source, "<.*?>|&.*?;", string.Empty);
        }
    }
}
