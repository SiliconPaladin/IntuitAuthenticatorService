using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    internal sealed class PrivateUserAccount : UserAccount
    {
        private byte[] PasswordHash { get; set; }

        private PasswordResetRequest PasswordResetRequest { get; set; }

        internal PrivateUserAccount(string firstName, string lastName, string emailAddress, string password)
            : base(firstName, lastName, emailAddress)
        {
            PasswordHash = password.GetPasswordHash();
        }

        internal bool ValidatePassword(string password) => password?.GetPasswordHash().IsEqualTo(PasswordHash) ?? false;

        internal int RequestPasswordReset()
        {
            PasswordResetRequest = new PasswordResetRequest();
            return PasswordResetRequest.RequestId;
        }

        internal AuthenticationStatus ResetPassword(int requestId, string password)
        {
            try
            {
                if(PasswordResetRequest == null)
                {
                    return AuthenticationStatus.ResetRequestNotFound;
                }
                else if(PasswordResetRequest.RequestId != requestId)
                {
                    return AuthenticationStatus.Failed;
                }
                else if((DateTime.UtcNow - PasswordResetRequest.RequestTime).TotalHours > Configuration.AllowedPasswordResetHoursSkew)
                {
                    return AuthenticationStatus.RequestIsTooOld;
                }

                PasswordHash = password.GetPasswordHash();
                return AuthenticationStatus.Success;
            }
            finally
            {
                PasswordResetRequest = null;
            }
        }
    }
}
