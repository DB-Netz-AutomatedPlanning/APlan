using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.HelperClasses
{
    internal static class FileExtensions
    {
        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return String.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
