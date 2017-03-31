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

        public void SelectOrder(OrderType orderType)
        {
            if (orderType != GetSelectedOrder())
            {
                _driver.Click(OrderProductLocator);
                switch (orderType)
                {
                    case OrderType.Popular:
                        _driver.Click(PopularSelectLocator);
                        break;
                    case OrderType.Cheap:
                        _driver.Click(CheapSelectLocator);
                        break;
                    case OrderType.Expensive:
                        _driver.Click(ExpensiveSelectLocator);
                        break;
                    case OrderType.Newest:
                        _driver.Click(NewSelectLocator);
                        break;
                    case OrderType.Raiting:
                        _driver.Click(RaitingSelectLocator);
                        break;
                    default:
                        break;
                }
            }
        }

        public OrderType GetSelectedOrder()
        {
            Dictionary<By, OrderType> orderTypes = new Dictionary<By, OrderType>()
            {
                {PopularSelectLocator, OrderType.Popular },
                {CheapSelectLocator, OrderType.Cheap},
                {ExpensiveSelectLocator, OrderType.Expensive},
                {NewSelectLocator, OrderType.Newest},
                {RaitingSelectLocator, OrderType.Raiting},
            };

            foreach (var item in orderTypes)
            {
                if (_driver.CheckContainsClass(item.Key, "schema-order__item_active"))
                {
                    return item.Value;
                }
            }
            return OrderType.Popular;
        }

        public enum OrderType
        {
            Popular, Cheap, Expensive, Newest, Raiting
        }

        public void SetMinPrice(double minPrice)
        {
            _driver.SendKeys(InputPriceFromLocator, minPrice.ToString());
        }

        public void SetMaxPrice(double maxPrice)
        {
            _driver.SendKeys(InputPriceToLocator, maxPrice.ToString());
        }

        public void SetMinMaxPrice(double minPrice, double maxPrice)
        {
            SetMinPrice(minPrice);
            SetMaxPrice(maxPrice);
        }

        public double[] GetPrices()
        {
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
                list.Add(item["full_name"].ToString().Replace("&quot;", "\"").Replace("&#039;", "'"));
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
