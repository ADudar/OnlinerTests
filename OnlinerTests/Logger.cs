using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;
using System.Threading;
using System.IO;

namespace OnlinerTests
{
    public class Logger
    {
        //protected ExtentReports _extent = new ExtentReports();
        protected static  ExtentTest _test;
        private static ThreadLocal<ExtentTest> _thread;
        private static readonly ExtentReports _instance = new ExtentReports();

        public static ExtentReports Instance
        {
            get
            {
                return _instance;
            }
        }

        public Logger() { }

        static  Logger()
        {
            string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = path.Substring(0, path.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            Directory.CreateDirectory(projectPath + "Reports");

            string reportPath = projectPath + "Reports\\Report" + TestContext.CurrentContext.Test.FullName + ".html";
            Instance.AttachReporter(new ExtentHtmlReporter(reportPath));
        }

        public static ExtentTest GetTest(string text)
        {
            //CreateTest(text);
            return _thread.Value;
        }

        public static void CreateTest(string text)
        {
            //_test = Instance.CreateTest(text);
            if (_thread== null)
                _thread = new ThreadLocal<ExtentTest>();

            var t = Instance.CreateTest(text);
            _thread.Value = t;
            //return t;
            _test = t;
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
                Instance.Flush();
        }

        public void TearDown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "<pre>" +  TestContext.CurrentContext.Result.StackTrace + "</pre>";
            var testMessage = TestContext.CurrentContext.Result.Message;
            if (testStatus == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Fail(stackTrace + testMessage);
            }
            else if (testStatus == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                Pass(testMessage);
            }
        }
    }
}
