using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    static class Log
    {
        private static string _logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "WebServiceLog.txt");

        internal static void WriteLine(string message, bool outputToConsole = false)
        {
            var now = DateTime.UtcNow;
            File.AppendAllLines(_logFilePath, new string[] { $"[{now.ToShortDateString()} {now.ToShortTimeString()}] {message}"});
            if(outputToConsole)
            {
                Console.WriteLine(message);
            }
        }
    }
}
