﻿using System;
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
        public IWebDriver Driver { get; }
        public IWait<IWebDriver> _wait;
        public DesiredCapabilities capabilities;
        Logger _logger;

        public WebDriver(string browser, Logger logger)
        {
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
            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            _wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        }

        public void Quit()
        {
            _logger.Info("quit success");
            //_logger.Flush();
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
            _logger.Info("finded element: " + element.TagName + " ,is enabled: " + element.Enabled);
            return element;
        }

        public IList<IWebElement> FindAllElementsWithWaiting(By locator)
        {
            _logger.Info("wait all elements from locator " + locator);
            var collection = _wait.Until(d => d.FindElements(locator));
            _logger.Info("count finded elements: " + collection.Count);
            return collection;
        }

        public void WaitWhileElementClassContainsText(By locator, string text)
        {
            _logger.Info("wait element from locator: " + locator);
            bool result = _wait.Until(d => !d.FindElement(locator).GetAttribute("class").Contains(text));
            _logger.Info("result of waitingn element" + (result));

        }
    }
}
