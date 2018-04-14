using System;
using System.Text.RegularExpressions;

namespace IntuitAuthenticatorService
{
    internal class Validation
    {
        private static Regex _simpleEmailRegex = new Regex(@"^[a-zA-Z0-9\.]+@[a-z0-9]+\.[a-z]+$");

        internal static bool IsValidEmailAddress(string emailAddress)
        {
            if(emailAddress == null)
            {
                return false;
            }
            return _simpleEmailRegex.IsMatch(emailAddress);
        }

        internal static bool ValidatePassword(string password)
        {
            if(String.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if(password.Length < 8)
            {
                return false;
            }

            return true;
        }
    }
}