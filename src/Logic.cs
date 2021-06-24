using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPKIView;
using MJBLogger;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;

namespace PKIKeyRecovery
{
    public partial class Form1
    {
        internal static Random random = new Random();

        internal string RandomString()
        {
            return new string(Enumerable.Repeat(RuntimeContext.CharSet, RuntimeContext.Conf.PasswordLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
