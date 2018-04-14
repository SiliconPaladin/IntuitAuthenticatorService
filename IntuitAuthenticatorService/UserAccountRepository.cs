using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    static class UserAccountRepository 
    {
        private static Dictionary<string, PrivateUserAccount> _repository = null;

        static UserAccountRepository()
        {
            _repository = new Dictionary<string, PrivateUserAccount>();
            AddUserAccount(new PrivateUserAccount("John", "Doe", "jdoe@unknown.org", "who#Am1?"));
            AddUserAccount(new PrivateUserAccount("Leia", "Organa", "leia@force.net", "URmy0nlyHope!"));
        }

        internal static AuthenticationStatus AddUserAccount(PrivateUserAccount user)
        {
            if(DoesUserExist(user.EmailAddress))
            {
                return AuthenticationStatus.UserAlreadyExists;
            }

            _repository.Add(user.EmailAddress.ToLowerInvariant(), user);

            return AuthenticationStatus.Success;
        }

        internal static bool DoesUserExist(string emailAddress)
        {
            return _repository.ContainsKey(emailAddress.ToLowerInvariant());
        }

        internal static PrivateUserAccount GetUser(string emailAddress)
        {
            if(!DoesUserExist(emailAddress))
            {
                return null;
            }
            else
            {
                return _repository[emailAddress];
            }
        }

        internal static UserAccount GetPublicUser(string emailAddress)
        {
            if(!DoesUserExist(emailAddress))
            {
                return null;
            }
            else
            {
                var privateUser = _repository[emailAddress];
                return new UserAccount(privateUser.FirstName, privateUser.LastName, privateUser.EmailAddress);
            }
        }

        internal static int RequestPasswordReset(string emailAddress)
        {
            if(!DoesUserExist(emailAddress))
            {
                throw new UserNotFoundException(emailAddress);
            }
            else
            {
                return _repository[emailAddress].RequestPasswordReset();
            }
        }

        internal static AuthenticationStatus ResetPassword(string emailAddress, int requestId, string password)
        {
            if(!DoesUserExist(emailAddress))
            {
                return AuthenticationStatus.Failed;
            }

            return _repository[emailAddress].ResetPassword(requestId, password);
        }
    }
}
