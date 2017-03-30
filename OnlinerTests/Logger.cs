using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;
using System.Threading;
using System.Management;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.CompilerServices;

namespace OnlinerTests
{
    public class Logger
    {
        //protected ExtentReports _extent = new ExtentReports();
        protected static  ExtentTest _test;
        private static ThreadLocal<ExtentTest> _thread;
        private static readonly ExtentReports _instance = new ExtentReports();
        private static ExtentReports _extent = Logger.Instance;

        public static ExtentReports Instance
        {
            get
            {
                return _instance;
            }
        }


        public  Logger()
        {
            string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = path.Substring(0, path.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            Directory.CreateDirectory(projectPath + "Reports");

            string reportPath = projectPath + "Reports\\Report" + TestContext.CurrentContext.Test.FullName + ".html";
            Instance.AttachReporter(new ExtentHtmlReporter(reportPath));

        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void  CreateTest(string text)
        {
            //_test = Instance.CreateTest(text);
            if (_thread== null)
                _thread = new ThreadLocal<ExtentTest>();

            var t = Instance.CreateTest(text);
            _thread.Value = t;

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
            lock (this)
            {
                Instance.Flush();
            }
        }
    }
}
