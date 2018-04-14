using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntuitAuthenticatorService.UnitTests
{
    [TestClass]
    public class PrivateUserAccountTests
    {
        const string DefaultFirstName = "User";
        const string DefaultLastName = "One";
        const string DefaultEmailAddress = "user.one@intuit.com";
        const string DefaultPassword = "P@ssw0rd";
        const string NewPassword = "NewP@ssw0rd";

        PrivateUserAccount defaultAccount = null;

        [TestInitialize]
        public void TestInitialize()
        {
            defaultAccount = new PrivateUserAccount(DefaultFirstName, DefaultLastName, DefaultEmailAddress, DefaultPassword);
        }

        [TestMethod]
        public void TestValidatePassword()
        {
            Assert.IsTrue(defaultAccount.ValidatePassword(DefaultPassword), "Correct password fails.");
            Assert.IsFalse(defaultAccount.ValidatePassword("AnotherPassword"), "Incorrect password authenticates.");
            Assert.IsFalse(defaultAccount.ValidatePassword(""), "Empty password authenticates.");
            Assert.IsFalse(defaultAccount.ValidatePassword(null), "Null password authenticates.");
        }

        [TestMethod]
        public void TestPasswordReset()
        {
            var resetId = defaultAccount.RequestPasswordReset();
            Assert.AreEqual(AuthenticationStatus.Success, defaultAccount.ResetPassword(resetId, NewPassword));
            Assert.IsFalse(defaultAccount.ValidatePassword(DefaultPassword));
            Assert.IsTrue(defaultAccount.ValidatePassword(NewPassword));
        }

        [TestMethod]
        public void TestPasswordInvalidReset()
        {
            var resetId = defaultAccount.RequestPasswordReset();
            Assert.AreEqual(AuthenticationStatus.Failed, defaultAccount.ResetPassword(++resetId, NewPassword));
        }

        [TestMethod]
        public void TestPasswordResetNotRequested()
        {
            Assert.AreEqual(AuthenticationStatus.ResetRequestNotFound, defaultAccount.ResetPassword(0, NewPassword));
        }

        [TestMethod]
        public void TestPasswordResetMultipleRequests()
        {
            var requestId1 = defaultAccount.RequestPasswordReset();
            var requestId2 = defaultAccount.RequestPasswordReset();
            Assert.AreNotEqual(requestId1, requestId2);
        }
    }
}
