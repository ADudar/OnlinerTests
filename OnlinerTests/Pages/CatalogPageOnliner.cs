using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace OnlinerTests.Pages
{
    public class CatalogPageOnliner
    {
        protected WebDriver _driver;

        public CatalogPageOnliner(WebDriver driver)
        {
            _driver = driver;
        }

        #region locators
        public By NotebooksLinkLocator { get; set; } = By.XPath("//a[@href='https://catalog.onliner.by/notebook']/span/span[@class='project-navigation__sign']");
        public static By PricesItemsLocator { get; set; } = By.XPath("//a[contains(@class, 'schema-product__price-value_primary')]/span[contains(@data-bind,'minPrice')]");
        public By LoadingProductLocator { get; set; } = By.CssSelector(".schema-products");
        public static  By RaitingItemsLocator { get; set; } = By.CssSelector(".rating");
        public By SchemaFilterButtonLocator { get; set; } = By.CssSelector(".schema-filter-button__state_initial");
        public By FullNameItemsLocator { get; set; } = By.XPath("//span[ contains(@data-bind,'product.extended_name')]");
        public By DescriptionItemsLocator { get; set; } = By.CssSelector(".schema-product__description>span");
        #endregion

        

        internal IList<string> GetFullNames(string v)
        {
            HttpWebRequest request = WebRequest.CreateHttp(v);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            WebResponse response = (HttpWebResponse)request.GetResponse();
            string text;
            using (var sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                text = sr.ReadToEnd();
            }
            IList<string> list = new List<string>(30);
            JObject json = JObject.Parse(text);

            foreach (var item in json["products"])
            {
                list.Add(item["extended_name"].ToString().Replace("&quot;", "\"").Replace("&#039;", "'"));
            }
            return list;
        }

        internal IList<string> GetFullNames(By locator)
        {
            _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
            _driver.WaitWhileElementClassContainsText(SchemaFilterButtonLocator, "schema-filter-button__state_animated");
            IList<IWebElement> fullnamesList = _driver.FindAllElementsWithWaiting(locator);
            IList<string> fullnames = new List<string>(fullnamesList.Count);
            foreach (var item in fullnamesList)
            {
                fullnames.Add(item.GetAttribute("innerHTML"));
            }
            return fullnames;
        }

        internal  double[] GetRatings()
        {
            IList<IWebElement> ratingList = _driver.FindAllElementsWithWaiting(RaitingItemsLocator);
            double[] ratings = new double[ratingList.Count];
            int i = 0;
            foreach (var item in ratingList)
            {
                string classname = item.GetAttribute("class");
                int pos = classname.IndexOf('_');
                ratings[i++] = Convert.ToInt32(classname.Substring(pos + 1, classname.Length - (pos + 1)));
            }
            return ratings;
        }

        internal  double[] GetPrices()
        {
            IList<IWebElement> pricesList = _driver.FindAllElementsWithWaiting(PricesItemsLocator);

            double[] pricesArray = new double[pricesList.Count];
            int i = 0;

            foreach (var item in pricesList)
            {
                string processedItem = item.GetAttribute("innerHTML").Replace("&nbsp;", "").Replace("р.", "").Replace(',', '.');
                pricesArray[i++] = Convert.ToDouble(processedItem);
            }
            return pricesArray;
        }

        internal void WaitAjaxResponse()
        {
            try
            {
                _driver.WaitElement(By.CssSelector(".schema-products_processing"));
                _driver.WaitElement(By.CssSelector(".schema-filter-button__state_animated"));
                _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
                _driver.WaitWhileElementClassContainsText(SchemaFilterButtonLocator, "schema-filter-button__state_animated");
            }
            catch  {}
        }

        internal string[] GetDescription()
        {
            IList<IWebElement> descriptionList = _driver.FindAllElementsWithWaiting(DescriptionItemsLocator);
            string[] pricesArray = new string[descriptionList.Count];
            int i = 0;
            foreach (var item in descriptionList)
            {
                pricesArray[i++] = item.GetAttribute("innerHTML");
            }
            return pricesArray;
        }
    }
}
