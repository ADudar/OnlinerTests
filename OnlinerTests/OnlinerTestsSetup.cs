using NUnit.Framework;
using System.Configuration;
using OnlinerTests.Pages;
using AventStack.ExtentReports.Reporter;
using Microsoft.CSharp.RuntimeBinder;
using AventStack.ExtentReports;

namespace OnlinerTests
{
    public class OnlinerTestsSetup
    {
        protected WebDriver _webDriver;
        protected Logger log;
        protected ExtentReports er = new ExtentReports();
        object myLock = new object();

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            //lock (myLock)
            //{

            log = new Logger();
            //}
        }

        [SetUp]
        public void Setup()
        {
            log.CreateTest(TestContext.CurrentContext.Test.MethodName);
            _webDriver = new WebDriver(log);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            lock (myLock)
            {
                log.Flush();
            }

            //todo: one report when run parallel
        }
    }
}
