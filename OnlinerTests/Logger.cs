using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;
using System.Threading;
using System.Management;
using System.CodeDom.Compiler;

namespace OnlinerTests
{
    public class Logger
    {
        protected ExtentReports _extent;
        protected ExtentTest _test;

        public Logger()
        {
            
            this._extent = new ExtentReports();
            this._extent.AddSystemInfo("OS", "" + Environment.OSVersion.Platform.ToString() + " " + Environment.OSVersion.VersionString);
            this._extent.AddSystemInfo("Selenium Version", "3.30");
            this._extent.AddSystemInfo("Author", Environment.UserName);
            var dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = TestContext.CurrentContext.Test.ClassName + ".html";
            this._extent.AttachReporter(new ExtentHtmlReporter(dir + fileName));
        }

        public void CreateTest(string text)
        {
            this._test = this._extent.CreateTest(text);
        }

        public void Info(string text)
        {
            this._test.Info(text);
        }

        public void Debug(string text)
        {
            this._test.Debug(text);
        }

        public void Error(string text)
        {
            this._test.Error(text);
        }

        public void Fail(string text)
        {
            this._test.Fail(text);
        }

        public void Pass(string text)
        {
            _test.Pass(text);
        }

        public void Log(Status s, string text)
        {
            this._test.Log(s, text);
        }

        public void Flush()
        {
            this._extent.Flush();
        }
    }
}
