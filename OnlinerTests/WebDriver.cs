using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Configuration;

namespace OnlinerTests
{
    public class WebDriver
    {
        public IWebDriver Driver { get; set; }
        public IWait<IWebDriver> _wait;
        public DesiredCapabilities capabilities;
        Logger _logger;

        public WebDriver(Logger logger)
        {
            string browser = ConfigurationManager.AppSettings["Browser"];
            _logger = logger;
            if (ConfigurationManager.AppSettings["GridEnabled"] == "true")
            {
                capabilities = new DesiredCapabilities();
                switch (browser)
                {
                    case "Chrome":
                        _logger.Info("Webdriver run with grid Chrome");
                        capabilities = DesiredCapabilities.Chrome();
                        break;
                    case "IE":
                    case "Internet Explorer":
                        _logger.Info("Webdriver run with grid IE");
                        capabilities = DesiredCapabilities.InternetExplorer();
                        break;
                    case "Firefox":
                        _logger.Info("Webdriver run with grid Firefox");
                        capabilities = DesiredCapabilities.Firefox();
                        break;
                    default:
                        _logger.Info("Webdriver run with grid default browser Chrome");
                        capabilities = DesiredCapabilities.Chrome();
                        break;
                }
                capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                Driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capabilities, TimeSpan.FromSeconds(600));
            }
            else
            {
                switch (browser)
                {
                    case "Chrome":
                        _logger.Info("Webdriver run with Chrome");
                        Driver = new ChromeDriver();
                        break;
                    case "IE":
                    case "Internet Explorer":
                        _logger.Info("Webdriver run with IE");
                        Driver = new InternetExplorerDriver();
                        break;
                    case "Firefox":
                        _logger.Info("Webdriver run with Firefox");
                        Driver = new FirefoxDriver();
                        break;
                    default:
                        _logger.Info("Webdriver run with default browser Chrome");
                        Driver = new ChromeDriver();
                        break;
                }
            }
            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            _wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            _wait.PollingInterval = TimeSpan.FromSeconds(1);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Driver.Manage().Window.Maximize();
        }

        internal void WaitPageLoad()
        {
            _wait.Until(_driver => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public void Quit()
        {
            Driver.Quit();
            _logger.Info("quit success");
        }

        public void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);
            _logger.Info("Navigate to " + url);
        }

        public void SendKeys(By locator, string text)
        {
            var element = WaitElementVisible(locator);
            _logger.Info("send keys to element: " + element.TagName + " value: " + text);
            element.SendKeys(text);
        }

        public void Click(By locator)
        {
            var element = Driver.FindElement(locator);
            _logger.Info("click to element: " + element.TagName + " locator: " + locator);
            element.Click();
        }

        public string GetText(By locator)
        {
            string findedText = WaitElementVisible(locator).Text;
            _logger.Info("finded text: " + findedText);
            return findedText;
        }

        public IWebElement WaitElementVisible(By locator)
        {
            var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
            _logger.Info("finded element from locator: " + locator + ", tagName: " + element.TagName + ", is enabled: " + element.Enabled);
            return element;
        }

        internal IWebElement WaitElementClickable(By locator)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            _logger.Info("finded element from locator: " + locator + ", tagName: " + element.TagName + ", is enabled: " + element.Enabled);
            return element;
        }

        public IList<IWebElement> FindAllElementsWithWaiting(By locator)
        {
            _logger.Info("wait all elements from locator " + locator);
            try
            {
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
            }
            catch { }
            var collection = _wait.Until(d => d.FindElements(locator));
            _logger.Info("count finded elements: " + collection.Count);
            return collection;
        }

        public bool WaitWhileElementClassNotContainsText(By locator, string text)
        {
            _logger.Info("wait element from locator: " + locator);
            bool result = _wait.Until(d => !d.FindElement(locator).GetAttribute("class").Contains(text));
            _logger.Info("result of waitingn element: " + (result));
            return result;
        }

        public bool CheckContainsClass(By locator, string className)
        {
            bool result = Driver.FindElement(locator).GetAttribute("class").Contains(className);
            _logger.Info("result of find element" + (result));
            return result;
        }

        public void Scroll(int pixels)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript($"scroll(0, { pixels });");
        }
    }
}
