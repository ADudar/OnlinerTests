using NUnit.Framework;
using OnlinerCore;
using OnlinerCore.Pages;
using OnlinerTests.Tests;

namespace OnlinerTests.Login
{
    [TestFixture]
    [Parallelizable]
    public class LoginOnlinerTest : TestsSetup
    {
        [TestCaseSource("Credentials")]
        public void LoginOnliner(string username, string password)
        {
            var loginPage = new LoginComponent(_webDriver);
            loginPage.Open();
            User user1 = User.Create(username, password);
            loginPage.Login(user1);
            string loginTitle = _webDriver.GetText(loginPage.AccountTitleLocator);
            Assert.AreEqual("n2440175", username.Substring(0, username.IndexOf('@')));
            string message = "Login success with username: " + username + " and password " + password;
            log.Pass(message);
        }

        static object[] Credentials = {
        new string[] { "n2440175@mvrht.com", "hellohello" }
    };
    }


}
