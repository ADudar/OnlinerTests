using NUnit.Framework;
using System.Configuration;
using OnlinerTests.Pages;
using log4net.Config;
using log4net;
using System.Reflection;

[assembly: XmlConfigurator(Watch = true)]

namespace OnlinerTests
{
    public class FilterPricesOnlinerTestSetUp
    {

        protected readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected WebDriver _webDriver;

        [SetUp]
        public void Setup()
        {
            string browser = ConfigurationManager.AppSettings["Browser"];
            string user = ConfigurationManager.AppSettings["Username"];
            _webDriver = new WebDriver(browser);
            log.Info("Test started with Browser " + browser);
            var loginPage = new LoginPageOnliner(_webDriver);
            loginPage.Open();
            log.Info("log page open success");
            User user1 = User.Create(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
            loginPage.Login(user1);
            log.Info("login success with username: " + user1.Mailbox + " and password: " + user1.Password);

            //DesiredCapabilities capabilities = new DesiredCapabilities();
            //capabilities = DesiredCapabilities.Chrome();
            //capabilities.SetCapability(CapabilityType.BrowserName, "chrome");
            //capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
            //_webDriver.Driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capabilities);

        }
        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
            log.Info("quit webdriwer success");
        }
        //[OneTimeTearDown]
    }
}
