using System;
using System.Globalization;

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

        public static decimal ToInvariantDecimal(this string value) {
            decimal result;
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ? result : 0;
        }
    }
}