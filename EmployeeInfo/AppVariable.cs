using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfo
{
    class AppVariable
    {
        private static string password = EncryptDecryptClassLibrary.EncryptDecrypt.Decrypt(ConfigurationManager.AppSettings["Data"], "SailorIT");
        public static string ServerLogin = "server=" + ConfigurationManager.AppSettings["DBServer"] + ";database=" + ConfigurationManager.AppSettings["DB"] + ";uid=" + ConfigurationManager.AppSettings["User"] + ";pwd=" + password + "";
    }
}
