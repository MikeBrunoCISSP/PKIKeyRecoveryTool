using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PKIKeyRecovery
{
    internal static class Extensions
    {
        internal static Regex MatchHex = new Regex(RegexPatterns.NonHexChars);

        internal static bool IsValidEmail(this string expression)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(expression);
                return addr.Address == expression;
            }
            catch
            {
                return false;
            }
        }

        internal static string OnlyHex(this string expression)
        {
            return Regex.Replace(expression, RegexPatterns.NonHexChars, string.Empty);
        }
    }
}
