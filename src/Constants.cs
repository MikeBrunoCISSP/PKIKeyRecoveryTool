using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIKeyRecovery
{
    internal static class AppSettings
    {
        internal const string ADCS_Host = nameof(ADCS_Host);
        internal const string ADCS_CN = nameof(ADCS_CN);
        internal const string ADDS_Domain = nameof(ADDS_Domain);
        internal const string ADDS_ContainerDN = nameof(ADDS_ContainerDN);
        internal const string LogLevel = nameof(LogLevel);
        internal const string PC_PasswordDistributionMethod = nameof(PC_PasswordDistributionMethod);
        internal const string Mobile_PasswordDistributionMethod = nameof(Mobile_PasswordDistributionMethod);
        internal const string Legal_PasswordDistributionMethod = nameof(Legal_PasswordDistributionMethod);
        internal const string PC_UseStrongPasswords = nameof(PC_UseStrongPasswords);
        internal const string Mobile_UseStrongPasswords = nameof(Mobile_UseStrongPasswords);
        internal const string Legal_UseStrongPasswords = nameof(Legal_UseStrongPasswords);
        internal const string PC_AttachKeyToEmail = nameof(PC_AttachKeyToEmail);
        internal const string Mobile_AttachKeyToEmail = nameof(Mobile_AttachKeyToEmail);
        internal const string Legal_AttachKeyToEmail = nameof(Legal_AttachKeyToEmail);
        internal const string PC_KeyRetrievalLocation = nameof(PC_KeyRetrievalLocation);
        internal const string Mobile_KeyRetrievalLocation = nameof(Mobile_KeyRetrievalLocation);
        internal const string Legal_KeyRetrievalLocation = nameof(Legal_KeyRetrievalLocation);
        internal const string PC_DeleteKeyAfterSending = nameof(PC_DeleteKeyAfterSending);
        internal const string Mobile_DeleteKeyAfterSending = nameof(Mobile_DeleteKeyAfterSending);
        internal const string Legal_DeleteKeyAfterSending = nameof(Legal_DeleteKeyAfterSending);
        internal const string mailhost = nameof(mailhost);
        internal const string mailSender = nameof(mailSender);
        internal const string PC_Message = nameof(PC_Message);
        internal const string Mobile_Message = nameof(Mobile_Message);
        internal const string Legal_Message = nameof(Legal_Message);
        internal const string Legal_Email = nameof(Legal_Email);
        internal const string ADCS_Templates = nameof(ADCS_Templates);
        internal const string ADCS_Mobile_Templates = nameof(ADCS_Mobile_Templates);
    }

    internal static class Default
    {
        internal const int MinPasswordLength = 8;
        internal const int MaxPasswordLength = 20;
    }

    internal static class Constants
    {
        internal const string InvalidEmail = @"Invalid Email";
        internal const string ConfFile = @"KRTool.cfg";
        internal const string Unavailable = nameof(Unavailable);
    }

    internal static class RegexPatterns
    {
        internal const string NonHexChars = @"[^a-fA-F0-9]";
    }
}
