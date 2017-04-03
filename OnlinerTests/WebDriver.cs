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
        public IWebDriver Driver { get; }
        public IWait<IWebDriver> _wait;
        public DesiredCapabilities capabilities;
        Logger _logger;

        public WebDriver(Logger logger)
        {
            string browser = ConfigurationManager.AppSettings["Browser"];
            _logger = logger;
            if (ConfigurationManager.AppSettings["GridEnabled"] == "true")
            {
                _logger.Info("Webdriver run from grid");
                capabilities = new DesiredCapabilities();
                switch (browser)
                {
                    case "Chrome":
                        _logger.Info("Webdriver run with Chrome");
                        capabilities = DesiredCapabilities.Chrome();
                        break;
                    case "IE":
                    case "Internet Explorer":
                        _logger.Info("Webdriver run with IE");
                        capabilities = DesiredCapabilities.InternetExplorer();
                        break;
                    case "Firefox":
                        _logger.Info("Webdriver run with Firefox");
                        capabilities = DesiredCapabilities.Firefox();
                        break;
                    default:
                        _logger.Info("Webdriver run with default browser Chrome");
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

        internal void WaitPageLoaded()
        {
            _wait.Until(_driver => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));

        }

        public void Quit()
        {
            _logger.Info("quit success");
            Driver.Quit();
        }

        public void Navigate(string url)
        {
            _logger.Info("Navigate to " + url);
            Driver.Navigate().GoToUrl(url);
        }

        public void SendKeys(By locator, string text)
        {
            var element = WaitElement(locator);
            _logger.Info("send keys to element: " + element.TagName + " value: " + text);
            element.SendKeys(text);
        }

        public void Click(By locator)
        {
            var element = WaitElement(locator);
            _logger.Info("click to element: " + element.TagName);
            element.Click();
        }

        public string GetText(By locator)
        {
            string findedText = WaitElement(locator).Text;
            _logger.Info("finded text: " + findedText);
            return findedText;
        }

        public IWebElement WaitElement(By locator)
        {
            _logger.Info("wait element from locator: " + locator);
            var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
            _logger.Info("finded element: " + element.TagName + ", is enabled: " + element.Enabled);
            return element;
        }

        public IList<IWebElement> FindAllElementsWithWaiting(By locator)
        {
            //Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
            _logger.Info("wait all elements from locator " + locator);
            try
            {
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
            }
            catch {}
            var collection = _wait.Until(d => d.FindElements(locator));
            _logger.Info("count finded elements: " + collection.Count);
            return collection;
        }

        public bool WaitWhileElementClassContainsText(By locator, string text)
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
            //jse.ExecuteScript("scroll(0," +  pixels + ");");
            jse.ExecuteScript($"scroll(0, { pixels });");
        }

        public IWebElement GetElementWithWaiting(By locator)
        {
            return WaitElement(locator);
        }


    }
}
