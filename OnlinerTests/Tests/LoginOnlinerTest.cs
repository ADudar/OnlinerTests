using NUnit.Framework;
using OnlinerTests.Pages;
using System.Configuration;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class LoginOnlinerTest : OnlinerTestsSetup
    {
        [Test]
        public void LoginOnliner()
        {
            var loginPage = new LoginPageOnliner(_webDriver);
            loginPage.Open();
            string username = ConfigurationManager.AppSettings["Username"];
            string password = ConfigurationManager.AppSettings["Password"];
            User user1 = User.Create(username, password);
            loginPage.Login(user1);
            string loginTitle = _webDriver.GetText(loginPage.AccountTitleLocator);
            Assert.AreEqual("n2440175", username.Substring(0, username.IndexOf('@')));
            string message = "Login success with username: " + username + " and password "+ password;
            log.Pass(message);
        }
    }
}
