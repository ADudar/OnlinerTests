using NUnit.Framework;
using System.Configuration;
using OnlinerTests.Pages;
using log4net.Config;
using log4net;
using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System;

namespace OnlinerTests
{
    public class FilterPricesOnlinerTestSetUp
    {
        protected readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected WebDriver _webDriver;
        protected ExtentReports _extent;

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            _extent = new ExtentReports();
            _extent.AddSystemInfo("OS", "Windows 10");
        }

        [SetUp]
        public void Setup()
        {
            string browser = ConfigurationManager.AppSettings["Browser"];
            _webDriver = new WebDriver(browser);
            log.Info("Test started with Browser " + browser);

            var loginPage = new LoginPageOnliner(_webDriver);
            loginPage.Open();
            log.Info("log page open success");
            User user1 = User.Create(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
            loginPage.Login(user1);
            log.Info("login success with username: " + user1.Mailbox + " and password: " + user1.Password);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
            log.Info("quit webdriwer success");
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            _extent.Flush();
        }
    }
}
