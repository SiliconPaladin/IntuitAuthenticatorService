using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    [DataContract]
    public class UserAccount
    {
        [DataMember]
        public string FirstName { get; private set; }

        [DataMember]
        public string LastName { get; private set; }

        [DataMember]
        public string EmailAddress { get; private set; }

        [OnDeserialized]
        void OnDeserialized(StreamingContext ctx)
        {
            if(String.IsNullOrWhiteSpace(FirstName) || 
               String.IsNullOrWhiteSpace(LastName) ||
               String.IsNullOrWhiteSpace(EmailAddress))
            {
                throw new InvalidOperationException("Required property is missing");
            }
        }

        public UserAccount()
        {
        }

        internal UserAccount(string firstName, string lastName, string emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress.ToLowerInvariant();
        }

        public override string ToString() => $"User Account {{ Name = {FirstName} {LastName}; Email = {EmailAddress} }}";
    }
}
