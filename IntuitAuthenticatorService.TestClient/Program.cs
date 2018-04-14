using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var newUserFirstName = "Geordi";
            var newUserLastName = "LaForge";
            var newUserEmailAddress = "glaforge@starfleet.org";
            var newUserPassword1 = "W@rpC0reBr33ch";
            var newUserPassword2 = "Red@1ert!";

            using(ChannelFactory<IAuthenticatorService> channelFactory = new ChannelFactory<IAuthenticatorService>(new WebHttpBinding(), "http://localhost:8080/authenticator"))
            {
                channelFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());
                IAuthenticatorService channel = channelFactory.CreateChannel();

                Console.Write($"Testing {nameof(channel.GetUserAccountInformation)} with unregistered user...");
                var userInfo = channel.GetUserAccountInformation(newUserEmailAddress);
                if(userInfo == null)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"The retrieved {nameof(UserAccount)} is not null.");
                    return;
                }

                Console.Write($"Testing {nameof(channel.AuthenticateUser)} with unregistered user...");
                var status = channel.AuthenticateUser(newUserEmailAddress, newUserPassword1);
                if(status == AuthenticationStatus.AuthenticationFailed)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"The unregistered user authenticated with return status {status} instead of expected value {AuthenticationStatus.AuthenticationFailed}.");
                    return;
                }

                Console.Write($"Testing {nameof(channel.CreateUser)} with new user...");
                status = channel.CreateUser(newUserEmailAddress, newUserFirstName, newUserLastName, newUserPassword1);
                if(status == AuthenticationStatus.Success)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"Failed to register the new user. Return status = {status}");
                    return;
                }

                Console.Write($"Testing {nameof(channel.AuthenticateUser)} with newly registered user...");
                status = channel.AuthenticateUser(newUserEmailAddress, newUserPassword1);
                if(status == AuthenticationStatus.Success)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"Authentication failed with expected good password. Return status = {status}.");
                    return;
                }

                Console.Write($"Testing {nameof(channel.AuthenticateUser)} with newly registered user and wrong password...");
                status = channel.AuthenticateUser(newUserEmailAddress, newUserPassword2);
                if(status == AuthenticationStatus.AuthenticationFailed)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"Authentication did not return expected status {AuthenticationStatus.AuthenticationFailed} with bad password. Return status = {status}.");
                    return;
                }

                Console.Write($"Testing {nameof(channel.GetUserAccountInformation)} with newly registered user...");
                userInfo = channel.GetUserAccountInformation(newUserEmailAddress);
                if(userInfo != null)
                {
                    Console.WriteLine("SUCCESS");
                    Console.WriteLine(userInfo.ToString());
                }
                else
                {
                    FailTest($"Failed to retrieve user account information.");
                    return;
                }

                Console.Write($"Testing {nameof(channel.RequestPasswordReset)} with newly registered user...");
                status = channel.RequestPasswordReset(newUserEmailAddress);
                if(status == AuthenticationStatus.Success)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"Failed to request password reset. Return status = {status}.");
                    return;
                }

                Console.Write($"Testing {nameof(channel.ResetPassword)} with newly registered user...");
                
                if(!File.Exists(IntuitAuthenticatorServiceInterface.SharedConstants.PasswordResetFilePath))
                {
                    Console.Write("Reset file path check...");
                    FailTest("File does not exist.");
                    return;
                }

                var passwordResetText = File.ReadAllText(IntuitAuthenticatorServiceInterface.SharedConstants.PasswordResetFilePath);
                if(!int.TryParse(passwordResetText, out int resetId))
                {
                    Console.Write("Retrieving reset ID...");
                    FailTest("Contents of reset file are not the expected integer format.");
                    return;
                }

                status = channel.ResetPassword(newUserEmailAddress, resetId, newUserPassword2);
                if(status == AuthenticationStatus.Success)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"Failed to reset password. Return status = {status}.");
                    return;
                }

                Console.Write($"Testing {nameof(channel.AuthenticateUser)} with new password...");
                status = channel.AuthenticateUser(newUserEmailAddress, newUserPassword2);
                if(status == AuthenticationStatus.Success)
                {
                    Console.WriteLine("SUCCESS");
                }
                else
                {
                    FailTest($"Authentication failed with expected good password. Return status = {status}.");
                    return;
                }

                Console.WriteLine("\n*** ALL TESTS COMPLETED SUCCESSFULLY! ***");
            }
        }

        private static void FailTest(string message)
        {
            Console.WriteLine("FAILED");
            Console.WriteLine(message);

            Console.WriteLine("\nTesting ABORTED.");
            Console.WriteLine("Press return to stop the client...");
            Console.ReadLine();
        }
    }
}
