using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntuitAuthenticatorService.UnitTests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void TestEmailValidation()
        {
            Assert.IsTrue(Validation.IsValidEmailAddress("chris.p.gardner@gmail.com"));
            Assert.IsTrue(Validation.IsValidEmailAddress("jdoe@unknown.org"));
            Assert.IsTrue(Validation.IsValidEmailAddress("1ab.23c@test.com"));
            Assert.IsFalse(Validation.IsValidEmailAddress("invalid"));
            Assert.IsFalse(Validation.IsValidEmailAddress("bad@domain"));
            Assert.IsFalse(Validation.IsValidEmailAddress(""));
        }

        public void TestEmailValidation_Null()
        {
            Assert.IsFalse(Validation.IsValidEmailAddress(null));
        }
    }
}
