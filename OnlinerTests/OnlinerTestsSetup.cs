using NUnit.Framework;
using System.Configuration;
using OnlinerTests.Pages;

namespace OnlinerTests
{
    public class OnlinerTestsSetup
    {
        protected WebDriver _webDriver;
        protected Logger log;

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            log = new Logger();

        }

        [SetUp]
        public void Setup()
        {
            log.CreateTest(TestContext.CurrentContext.Test.MethodName);
            string browser = ConfigurationManager.AppSettings["Browser"];
            _webDriver = new WebDriver(browser, log);
            var loginPage = new LoginPageOnliner(_webDriver);
            loginPage.Open();
            User user1 = User.Create(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
            loginPage.Login(user1);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            log.Flush();
            //todo: one report when run parallel
        }
    }
}
