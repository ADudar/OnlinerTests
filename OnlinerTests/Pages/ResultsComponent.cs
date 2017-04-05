using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Linq;

namespace OnlinerTests.Pages
{
    public class ResultsComponent
    {
        protected WebDriver _driver;

        public ResultsComponent(WebDriver driver)
        {
            _driver = driver;
        }

        #region locators
        public By PricesItemsLocator { get; set; } = By.XPath("//a[contains(@class, 'schema-product__price-value_primary')]/span[contains(@data-bind,'minPrice')]");
        public By LoadingProductLocator { get; set; } = By.CssSelector(".schema-products");
        public By RaitingItemsLocator { get; set; } = By.CssSelector(".rating");
        public By SchemaFilterButtonLocator { get; set; } = By.CssSelector(".schema-filter-button__state_initial");
        public By FullNameItemsLocator { get; set; } = By.XPath("//span[ contains(@data-bind,'product.extended_name')]");
        public By DescriptionItemsLocator { get; set; } = By.CssSelector(".schema-product__description>span");
        #endregion

        internal IList<string> GetFullNamesFromUrl(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            WebResponse response = (HttpWebResponse)request.GetResponse();
            string text;
            using (var sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                text = sr.ReadToEnd();
            }
            JObject json = JObject.Parse(text);
            return json["products"].Select(item => 
            item["extended_name"].ToString().Replace("&quot;", "\"").Replace("&#039;", "'")).ToList();
        }

        internal IList<string> GetFullNamesFromLocator(By locator)
        {
            IList<IWebElement> fullnamesList = _driver.FindAllElementsWithWaiting(locator);
            return fullnamesList.Select(el => el.GetAttribute("innerHTML")).ToList();
        }

        internal  double[] GetRatings()
        {
            IList<IWebElement> ratingList = _driver.FindAllElementsWithWaiting(RaitingItemsLocator);
            return ratingList.Select(el => 
            Convert.ToDouble(el.GetAttribute("class")
            .Substring(el.GetAttribute("class").IndexOf('_') + 1, el.GetAttribute("class").Length -(el.GetAttribute("class").IndexOf('_')+1) ))).ToArray();
        }

        internal  double[] GetPrices()
        {
            IList<IWebElement> pricesList = _driver.FindAllElementsWithWaiting(PricesItemsLocator);
            return pricesList.Select(el => 
                    Convert.ToDouble(el.GetAttribute("innerHTML")
                                       .Replace("&nbsp;", "").Replace("р.", "")
                                       .Replace(',', '.'))).ToArray();
        }

        internal void WaitPageLoad()
        {
            try
            {
                _driver.WaitElementVisible(By.CssSelector(".schema-products_processing"));
                _driver.WaitElementVisible(By.CssSelector(".schema-filter-button__state_animated"));
                _driver.WaitWhileElementClassNotContainsText(LoadingProductLocator, "schema-products_processing");
                _driver.WaitWhileElementClassNotContainsText(SchemaFilterButtonLocator, "schema-filter-button__state_animated");
            }
            catch  {}
        }

        internal string[] GetDescription()
        {
            IList<IWebElement> descriptionList = _driver.FindAllElementsWithWaiting(DescriptionItemsLocator);
            return descriptionList.Select(el => el.GetAttribute("innerHTML")).ToArray();
        }

        public void Open(string url)
        {
            _driver.Navigate(url);
        }
    }
}
