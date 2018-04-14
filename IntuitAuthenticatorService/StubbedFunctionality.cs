using IntuitAuthenticatorServiceInterface;
using System;
using System.IO;

namespace IntuitAuthenticatorService
{
    static class StubbedFunctionality
    {
        /// <summary>
        /// Used to fake out e-mail functionality of a password reset request message. Instead of email being sent,
        /// the request ID is saved as a text file which the test client can read.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="resetId"></param>
        internal static void SendPasswordResetEmail(string emailAddress, int resetId)
        {
            if(!Directory.Exists(Path.GetDirectoryName(SharedConstants.PasswordResetFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SharedConstants.PasswordResetFilePath));
            }

            File.WriteAllText(SharedConstants.PasswordResetFilePath, resetId.ToString());
        }
    }
}