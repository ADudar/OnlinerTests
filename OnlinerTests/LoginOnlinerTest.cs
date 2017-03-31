using NUnit.Framework;
using OnlinerTests.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
