using NUnit.Framework;
using System.Configuration;
using OnlinerTests.Pages;
using AventStack.ExtentReports.Reporter;
using Microsoft.CSharp.RuntimeBinder;

namespace OnlinerTests
{
    public class OnlinerTestsSetup
    {
        protected WebDriver _webDriver;
        protected Logger log;
        object myLock = new object();

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            lock (myLock)
            {
                log = new Logger();
            }
        }

        [SetUp]
        public void Setup()
        {

            log.CreateTest(TestContext.CurrentContext.Test.MethodName);
            _webDriver = new WebDriver(log);
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
