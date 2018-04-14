using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    static class Configuration
    {
        private static int _passwordResetSkew = 24;
        internal static int AllowedPasswordResetHoursSkew => _passwordResetSkew;

        static Configuration()
        {
            if(int.TryParse(System.Configuration.ConfigurationManager.AppSettings[nameof(AllowedPasswordResetHoursSkew)], out int tempInt))
            {
                _passwordResetSkew = tempInt;
            }
        }
    }
}
