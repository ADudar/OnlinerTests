using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Globalization;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace OnlinerTests.Pages
{
    public class CatalogPageOnliner
    {
        private WebDriver _driver;

        public CatalogPageOnliner(WebDriver driver)
        {
            _driver = driver;
        }

        public By NotebooksLinkLocator { get; set; } = By.XPath("//a[@href='https://catalog.onliner.by/notebook']/span/span[@class='project-navigation__sign']");

        public By InputPriceFromLocator { get; set; } = By.XPath("//input[contains(@class, 'schema-filter__number-input_price') and contains(@data-bind, 'value: facet.value.from')]");

        public By InputPriceToLocator { get; set; } = By.XPath("//input[contains(@class, 'schema-filter__number-input_price') and contains(@data-bind, 'value: facet.value.to')]");

        public By ItemsLocator { get; set; } = By.CssSelector(".schema-products");

        public By PricesNotebooksLocator { get; set; } = By.XPath("//a[contains(@class, 'schema-product__price-value_primary')]/span[contains(@data-bind,'minPrice')]");

        public By FilterPriceLocator { get; set; } = By.ClassName("schema-tags__text");

        public By LoadingProductLocator { get; set; } = By.CssSelector(".schema-products");

        public By OrderProductLocator { get; set; } = By.CssSelector(".schema-order__link");

        public By PopularSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(1)");

        public By CheapSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(2)");

        public By ExpensiveSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(3)");

        public By NewSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(4)");

        public By RaitingSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(5)");

        public By RaitingItemsLocator { get; set; } = By.CssSelector(".rating");

        public By SchemaFilterButtonLocator { get; set; } = By.CssSelector(".schema-filter-button__state_initial");

        public By FullNameItemsLocator { get; set; } = By.XPath("//span[ contains(@data-bind,'product.extended_name')]");

        public void NavigateToNotebooksPage()
        {
            _driver.Click(NotebooksLinkLocator);
        }

        public void SetPriceNotebooks(double price, By locator)
        {
            _driver.SendKeys(locator, price.ToString());
        }

        public void SelectOrder(By locator)
        {
            if (!_driver.CheckContainsClass(locator, "schema-order__item_active"))
            {
                _driver.Click(OrderProductLocator);
                _driver.Click(locator);
            }

        }

        public double[] GetPrices()
        {
            _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
            IList<IWebElement> pricesList = _driver.FindAllElementsWithWaiting(PricesNotebooksLocator);

            double[] pricesArray = new double[pricesList.Count];
            int i = 0;

            foreach (var item in pricesList)
            {
                string processedItem = item.GetAttribute("innerHTML").Replace("&nbsp;", "").Replace("р.", "").Replace(',', '.');
                pricesArray[i++] = Convert.ToDouble(processedItem);
            }
            return pricesArray;
        }

        internal int[] GetRatings()
        {
            _driver.WaitElement(By.CssSelector(".schema-products_processing"));
            _driver.WaitElement(By.CssSelector(".schema-filter-button__state_animated"));
            _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
            _driver.WaitWhileElementClassContainsText(SchemaFilterButtonLocator, "schema-filter-button__state_animated");
            IList<IWebElement> ratingList = _driver.FindAllElementsWithWaiting(RaitingItemsLocator);
            int[] ratings = new int[ratingList.Count];
            int i = 0;
            foreach (var item in ratingList)
            {
                string classname = item.GetAttribute("class");
                int pos = classname.IndexOf('_');
                ratings[i++] = Convert.ToInt32(classname.Substring(pos + 1, classname.Length - (pos + 1)));
            }
            return ratings;
        }

        public string ConvertToStringPriceWithFormat(double price)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)
            CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 0;
            return price.ToString("n", nfi);
        }

        internal IList<string> GetNotebooksFullNames(string v)
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
                list.Add(item["full_name"].ToString().Replace("&quot;","\"").Replace("&#039;","'"));
            }
            return list;
        }

        internal IList<string> GetNotebooksFullNames(By locator)
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

        public void Open()
        {
            _driver.Navigate("https://catalog.onliner.by/notebook");
        }
    }
}
