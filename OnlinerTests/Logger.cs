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
        private static string reportFileName = DateTime.Now.ToString("dd.MM.yyyy__HH.mm.ss");
        private static string dirAllReportsName = "Reports";
        private static string dirReportName = DateTime.Now.ToString("dd.MM.yyyy__HH.mm.ss");
        private static int screenshotIndex = 1;

        public Logger() { }

        static Logger()
        {
            string reportPath = Path.Combine(projectPath, dirAllReportsName, dirReportName, reportFileName) + ".html";
            _extent = new ExtentReports();
            _extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);
            _extent.AddSystemInfo("Author", Environment.UserName);
            var htmlreport = new ExtentHtmlReporter(reportPath);
            htmlreport.AppendExisting = false;
            htmlreport.LoadConfig(Path.Combine(projectPath, "Report.config"));
            _extent.AttachReporter(htmlreport);
        }

        public void WriteResults(IWebDriver _driver)
        {
            Directory.CreateDirectory(Path.Combine(projectPath, dirAllReportsName, dirReportName));
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "<pre>" + TestContext.CurrentContext.Result.StackTrace + "</pre>";
            var errorMessage = TestContext.CurrentContext.Result.Message;
            if (status == TestStatus.Failed)
            {
                _test.Fail(stackTrace + errorMessage);
                Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                string imageFilePath = Path.Combine(projectPath, dirAllReportsName, dirReportName, reportFileName + "_" + screenshotIndex++) + ".png";
                screenshot.SaveAsFile(imageFilePath, ScreenshotImageFormat.Png);
                _test.AddScreenCaptureFromPath(imageFilePath);
            }
            _test.Info("write results success");
        }

        public void CreateTest(string testname, string d = null)
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
