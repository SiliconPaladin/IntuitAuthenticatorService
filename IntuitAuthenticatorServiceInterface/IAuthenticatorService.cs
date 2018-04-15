using System.ServiceModel;
using System.ServiceModel.Web;

namespace IntuitAuthenticatorService
{
    [ServiceContract]
    public interface IAuthenticatorService
    {
        /// <summary>
        /// Authenticates a user via their email address and password
        /// </summary>
        /// <param name="emailAddress">The user's email address</param>
        /// <param name="password">The user's password</param>
        /// <returns>
        /// <see cref="AuthenticationStatus.Success"/> if user authenticated successfully;
        /// <see cref="AuthenticationStatus.InvalidEmailAddress"/> if an invalid email address is provided;
        /// otherwise <see cref="AuthenticationStatus.AuthenticationFailed"/> is returned.
        /// </returns>
        [OperationContract]
        [WebGet]
        AuthenticationStatus AuthenticateUser(string emailAddress, string password);

        /// <summary>
        /// Creates a new user and saves them to the user repository.
        /// </summary>
        /// <param name="emailAddress">The user's email address</param>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
        /// <param name="password">The user's password</param>
        /// <returns>
        /// <see cref="AuthenticationStatus.Success"/> if the user account was successfully created;
        /// <see cref="AuthenticationStatus.EmptyFirstName"/> if <paramref name="firstName"/> is null, empty, or whitespace;
        /// <see cref="AuthenticationStatus.EmptyLastName"/> if <paramref name="lastName"/> is null, empty, or whitespace;
        /// <see cref="AuthenticationStatus.InvalidEmailAddress"/> if <paramref name="emailAddress"/> is not a valid email address;
        /// <see cref="AuthenticationStatus.InvalidPassword"/> if <paramref name="password"/> is null or does not meet the password complexity requirements;
        /// <see cref="AuthenticationStatus.UserAlreadyExists"/> if a user with email address <paramref name="emailAddress"/> already exists in the system.
        /// </returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "create?email={emailAddress}&first={firstName}&last={lastName}&pwd={password}")]
        AuthenticationStatus CreateUser(string emailAddress, string firstName, string lastName, string password);

        /// <summary>
        /// Gets the user account for the user with the specified email address
        /// </summary>
        /// <param name="emailAddress">The user's email address</param>
        /// <returns>
        /// The user's account information if found; otherwise, <c>null</c> is returned.
        /// </returns>
        [OperationContract]
        [WebGet]
        UserAccount GetUserAccountInformation(string emailAddress);

        /// <summary>
        /// Requests a password reset.  The reset information will be emailed to the requested address.
        /// </summary>
        /// <param name="emailAddress">The user's email address</param>
        /// <returns>
        /// <see cref="AuthenticationStatus.Success"/> if the password reset request was successfully emailed to the user;
        /// otherwise, <see cref="AuthenticationStatus.Failed"/> is returned.
        /// </returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "requestReset?email={emailAddress}")]
        AuthenticationStatus RequestPasswordReset(string emailAddress);

        /// <summary>
        /// Resets a user's password using the last requested reset ID
        /// </summary>
        /// <param name="emailAddress">The user's email address</param>
        /// <param name="requestId">The integer ID provided in the reset request email to <paramref name="emailAddress"/></param>
        /// <param name="password">The new password.</param>
        /// <returns>
        /// <see cref="AuthenticationStatus.Success"/> if the password was successfully reset;
        /// <see cref="AuthenticationStatus.ResetRequestNotFound"/> if there was no request to reset the password or if the reset was invalidated previously;
        /// <see cref="AuthenticationStatus.RequestIsTooOld"/> if the reset was used after the allowed time skew has passed;
        /// otherwise, <see cref="AuthenticationStatus.Failed"/> is returned.
        /// </returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "resetPassword?email={emailAddress}&request={requestId}&pwd={password}")]
        AuthenticationStatus ResetPassword(string emailAddress, int requestId, string password);
    }
}