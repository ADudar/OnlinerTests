using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;
using System.IO;
using OpenQA.Selenium;
using NUnit.Framework.Interfaces;

namespace OnlinerTests
{
    public class Logger
    {
        private ExtentTest _test;
        private static ExtentReports _extent;
        private static string debugPath = TestContext.CurrentContext.TestDirectory;
        private static string projectPath = debugPath.Substring(0, debugPath.IndexOf("bin"));
        private static DateTime d = DateTime.Now;
        private static string fileName = d.Day + "." + d.Month + "." + d.Year + "__" + d.Hour + "_" + d.Minute + "_" + d.Second;
        static int i = 1;

        public Logger() { }

        static Logger()
        {
            string filepath = Path.Combine(projectPath, "Reports", fileName) + ".html";
            _extent = new ExtentReports();
            _extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);
            _extent.AddSystemInfo("Author", Environment.UserName);
            var htmlreport = new ExtentHtmlReporter(filepath);
            htmlreport.AppendExisting = false;
            htmlreport.LoadConfig(Path.Combine(projectPath, "Report.config"));
            _extent.AttachReporter(htmlreport);
        }

        public void WriteResults(IWebDriver _driver)
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "<pre>" + TestContext.CurrentContext.Result.StackTrace + "</pre>";
            var errorMessage = TestContext.CurrentContext.Result.Message;
            if (status == TestStatus.Failed)
            {
                _test.Fail( stackTrace + errorMessage );
                ITakesScreenshot ts = (ITakesScreenshot)_driver;
                Screenshot screenshot = ts.GetScreenshot();
                string imageFilePath = Path.Combine(projectPath, "Reports", fileName + "(" + i++ + ")") + ".png";
                screenshot.SaveAsFile(imageFilePath, ScreenshotImageFormat.Png);
                _test.AddScreenCaptureFromPath(imageFilePath);
            }
            _test.Info("write results success");
        }

        public  void CreateTest(string testname, string d = null)
        {
            _test = _extent.CreateTest(testname, d);
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
                _extent.Flush();
        }
    }
}
