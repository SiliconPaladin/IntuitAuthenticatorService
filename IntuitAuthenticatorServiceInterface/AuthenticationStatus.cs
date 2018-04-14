namespace IntuitAuthenticatorService
{
    public enum AuthenticationStatus
    {
        Success,
        AuthenticationFailed,
        UserAlreadyExists,
        UserNotFound,
        InvalidEmailAddress,
        InvalidPassword,
        EmptyFirstName,
        EmptyLastName,
        Failed,
        RequestIsTooOld,
        ResetRequestNotFound
    }
}