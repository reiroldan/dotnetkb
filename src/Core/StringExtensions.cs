using System;

namespace DotNetKillboard
{
    public static class StringExtensions
    {

        public static bool InsensitiveCompare(this string compare, string to) {
            return string.Compare(compare, to, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool IsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }
    }
}