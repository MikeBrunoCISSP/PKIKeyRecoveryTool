using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        internal static string Protect(this string plaintext)
        {
            return Convert.ToBase64String(
                ProtectedData.Protect(
                    Encoding.UTF8.GetBytes(plaintext)
                    , null
                    , DataProtectionScope.CurrentUser));
        }

        internal static string UnProtect(this string ciphertext)
        {
            return Encoding.UTF8.GetString(
                ProtectedData.Unprotect(
                    Convert.FromBase64String(ciphertext)
                    , null
                    , DataProtectionScope.CurrentUser));
        }
    }
}
