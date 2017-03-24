using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace OnlinerTests
{
    public enum BrowserType { Chrome, IE, Firefox };

    public class WebDriver
    {
        public IWebDriver Driver { get; set; }
        private IWait<IWebDriver> _wait;

        public WebDriver(string browser)
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
            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            _wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

        }

        public void Quit()
        {
            try
            {
                Driver.Quit();
            }
            catch
            {
                
                throw;
            }
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
            //_wait.Until(d => element.Displayed);
            return element;
        }

        //public IWebElement FindElementWithWaiting(By by)
        //{
        //    var element = _wait.Until(d => d.FindElement(by));
        //    _wait.Until(d => element.Displayed);
        //    return element;
        //}

        public IList<IWebElement> FindAllElementsWithWaiting(By locator)
        {
            return _wait.Until(d => d.FindElements(locator));
        }

        //public void WaitForElementIsVisible(IWebElement element)
        //{
        //    _wait.Until(d => element.Displayed);
        //}

        public void WaitWhileElementClassContainsText(By by, string text)
        {
            _wait.Until(d => !d.FindElement(by).GetAttribute("class").Contains(text));
        }
    }
}
