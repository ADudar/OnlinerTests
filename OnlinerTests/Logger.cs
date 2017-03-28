using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;

namespace OnlinerTests
{
    public class Logger
    {
        protected ExtentReports _extent;
        protected ExtentTest _test;

        public Logger()
        {
            var dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = this.GetType().ToString() + ".html";
            var htmlReporter = new ExtentHtmlReporter(dir + fileName);
            htmlReporter.Configuration().DocumentTitle = "filter price tests";
            _extent = new ExtentReports();
            _extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);
            _extent.AddSystemInfo("Selenium Version", "3.30");
            _extent.AddSystemInfo("Author", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            _extent.AttachReporter(htmlReporter);
        }

        public void CreateTest(string text)
        {
            _test = _extent.CreateTest(text);
        }

        public void Info(string text)
        {
            _test.Info(text);
        }

        public void Debug(string text)
        {
            _test.Debug(text);
        }

        public void Error(string text)
        {
            _test.Error(text);
        }

        public void Fail(string text)
        {
            _test.Fail(text);
        }

        public void Pass(string text)
        {
            _test.Pass(text);
        }

        public void Log(Status s, string text)
        {
            _test.Log(s, text);
        }

        public void Flush()
        {

            _test.Info("quit webdriver all tests success");
            if (_extent != null)
            {
                _extent.Flush();
            }

        }
    }
}
