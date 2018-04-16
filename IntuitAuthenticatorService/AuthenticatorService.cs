using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    sealed class AuthenticatorService : IAuthenticatorService
    {
        public AuthenticationStatus AuthenticateUser(string emailAddress, string password)
        {
            emailAddress = emailAddress?.ToLowerInvariant();
            Log.WriteLine($"Authenticating User {emailAddress}...");

            if(password == null)
            {
                Log.WriteLine($"Authenticating User {emailAddress} FAILED => NULL password");
                return AuthenticationStatus.AuthenticationFailed;
            }

            if(Validation.IsValidEmailAddress(emailAddress))
            {
                var user = UserAccountRepository.GetUser(emailAddress);
                if(user == null)
                {
                    Log.WriteLine($"Authenticating User {emailAddress} FAILED => User not found");
                    return AuthenticationStatus.AuthenticationFailed;
                }
                else
                {
                    if(!user.ValidatePassword(password))
                    {
                        Log.WriteLine($"Authenticating User {emailAddress} FAILED => Wrong password");
                        return AuthenticationStatus.AuthenticationFailed;
                    }
                    else
                    {
                        Log.WriteLine($"Authenticating User {emailAddress} SUCCESS!");
                        return AuthenticationStatus.Success;
                    }
                }
            }
            else
            {
                Log.WriteLine($"Authenticating User {emailAddress} FAILED => Invalid email address");
                return AuthenticationStatus.InvalidEmailAddress;
            }
        }

        public AuthenticationStatus CreateUser(string emailAddress, string firstName, string lastName, string password)
        {
            emailAddress = emailAddress?.ToLowerInvariant();
            Log.WriteLine($"Creating new user '{emailAddress}'...");
            if(String.IsNullOrWhiteSpace(firstName))
            {
                Log.WriteLine($"Creating new user '{emailAddress} FAILED => No first name");
                return AuthenticationStatus.EmptyFirstName;
            }
            else if(String.IsNullOrWhiteSpace(lastName))
            {
                Log.WriteLine($"Creating new user '{emailAddress} FAILED => No last name");
                return AuthenticationStatus.EmptyLastName;
            }
            else if(!Validation.IsValidEmailAddress(emailAddress))
            {
                Log.WriteLine($"Creating new user '{emailAddress} FAILED => Invalid email address");
                return AuthenticationStatus.InvalidEmailAddress;
            }
            else if(!Validation.ValidatePassword(password))
            {
                Log.WriteLine($"Creating new user '{emailAddress} FAILED => Invalid password");
                return AuthenticationStatus.InvalidPassword;
            }
            else if(UserAccountRepository.DoesUserExist(emailAddress))
            {
                Log.WriteLine($"Creating new user '{emailAddress} FAILED => User already exists");
                return AuthenticationStatus.UserAlreadyExists;
            }

            Log.WriteLine($"Creating new user '{emailAddress} SUCCESS!");
            var user = new PrivateUserAccount(firstName, lastName, emailAddress, password);
            return UserAccountRepository.AddUserAccount(user);
        }

        public UserAccount GetUserAccountInformation(string emailAddress) => UserAccountRepository.GetPublicUser(emailAddress?.ToLowerInvariant());

        public AuthenticationStatus RequestPasswordReset(string emailAddress)
        {
            emailAddress = emailAddress?.ToLowerInvariant();
            try
            {
                Log.WriteLine($"Requesting password reset for {emailAddress}...");
                var resetId = UserAccountRepository.RequestPasswordReset(emailAddress);
                Log.WriteLine($"Requesting password reset for {emailAddress} SUCCESS! [Reset ID = {resetId}]");
                StubbedFunctionality.SendPasswordResetEmail(emailAddress, resetId);
                return AuthenticationStatus.Success;
            }
            catch(UserNotFoundException)
            {
                Log.WriteLine($"Requesting password reset for {emailAddress} FAILED => User not found");
                return AuthenticationStatus.Failed;
            }
        }

        public AuthenticationStatus ResetPassword(string emailAddress, int requestId, string password) => UserAccountRepository.ResetPassword(emailAddress?.ToLowerInvariant(), requestId, password);
    }
}
