using NUnit.Framework;

namespace OnlinerTests
{
    public class OnlinerTestsSetup
    {
        protected WebDriver _webDriver;
        protected Logger log;

        //protected ExtentReports er = new ExtentReports();
        //private static ExtentReports _extent = Logger.Instance;
        //private static ThreadLocal<ExtentTest> _test;
        //object myLock = new object();

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            log = new Logger();
        }

        [SetUp]
        public void Setup()
        {
            Logger.CreateTest(TestContext.CurrentContext.Test.MethodName);
            _webDriver = new WebDriver(log);
        }

        [TearDown]
        public void TearDown()
        {
            log.TearDown();
            _webDriver.Quit();
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
                log.Flush();
        }
    }
}
