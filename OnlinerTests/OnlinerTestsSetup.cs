using NUnit.Framework;
using System.Configuration;
using OnlinerTests.Pages;
using AventStack.ExtentReports.Reporter;
using Microsoft.CSharp.RuntimeBinder;
using AventStack.ExtentReports;

namespace OnlinerTests
{
    public class OnlinerTestsSetup : OnlinerOneTime
    {
        [SetUp]
        public void Setup()
        {
            log.CreateTest(TestContext.CurrentContext.Test.Name);
            _webDriver = new WebDriver(log);
        }

        [TearDown]
        public void TearDown()
        {
            log.WriteResults(_webDriver.Driver);
            _webDriver.Quit();
        }


    }
}
