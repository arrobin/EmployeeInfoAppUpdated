using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfo.Helper
{
    class Helper
    {
        public static void WriteError(Exception exception)
        {
            string text = DateTime.Now.ToShortTimeString() + "\n" +
                "Source: " + exception.Source + "\n" +
                "Message: " + exception.Message + "\n" +
                "Stack Trace: " + exception.StackTrace + "\n" +
                "-----------------------------------------------------------------------";
            System.IO.StreamWriter file = new System.IO.StreamWriter("Errors\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true);
            file.WriteLine(text);
            file.Close();
        }
    }
}
