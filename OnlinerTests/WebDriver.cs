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
    public enum BrowserType { Chrome, IE, Firefox };

    public class WebDriver
    {
        public IWebDriver Driver { get; set; }
        private IWait<IWebDriver> _wait;

        public DesiredCapabilities capability;

        public WebDriver(string browser)
        {

            if (ConfigurationManager.AppSettings["GridEnabled"] == "true")
            {
                capability = new DesiredCapabilities();
                switch (browser)
                {
                    case "Chrome":
                        capability = DesiredCapabilities.Chrome();
                        break;
                    case "IE":
                    case "Internet Explorer":
                        capability = DesiredCapabilities.InternetExplorer();
                        break;
                    case "Firefox":
                        capability = DesiredCapabilities.Firefox();
                        break;
                    default:
                        capability = DesiredCapabilities.Chrome();
                        break;
                }
                capability.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                Driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));
            }
            else
            {
                switch (browser)
                {
                    case "Chrome":
                        Driver = new ChromeDriver();
                        break;
                    case "IE":
                    case "Internet Explorer":
                        Driver = new InternetExplorerDriver();
                        break;
                    case "Firefox":
                        Driver = new FirefoxDriver();
                        break;
                    default:
                        Driver = new ChromeDriver();
                        break;
                }
            }
            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            _wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        }

        public void Quit()
        {
                Driver.Quit();
        }

        public void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void SendKeys(By locator, string text)
        {
            var element = WaitElement(locator);
            element.SendKeys(text);
        }

        public void Click(By locator)
        {
            var element = WaitElement(locator);
            element.Click();
        }

        public string GetText(By locator)
        {
            return WaitElement(locator).Text;
        }

        public IWebElement WaitElement(By locator)
        {
            var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element;
        }

        public IList<IWebElement> FindAllElementsWithWaiting(By locator)
        {
            return _wait.Until(d => d.FindElements(locator));
        }

        public void WaitWhileElementClassContainsText(By by, string text)
        {
            _wait.Until(d => !d.FindElement(by).GetAttribute("class").Contains(text));
        }
    }
}
