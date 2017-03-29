using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Globalization;
using System.Threading;
using OpenQA.Selenium.Support.UI;

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


        public void NavigateToNotebooksPage()
        {
            _driver.Click(NotebooksLinkLocator);
        }

        public void SetMinPriceNotebooks(double minPrice)
        {
            _driver.SendKeys(InputPriceFromLocator, minPrice.ToString());
        }

        public void SetMaxPriceNotebooks(double maxPrice)
        {
            _driver.SendKeys(InputPriceToLocator, maxPrice.ToString());
        }

        public void SelectOrder(By locator)
        {
            _driver.Click(OrderProductLocator);
            _driver.Click(locator);
            //_driver.FindAllElementsWithWaiting(PricesNotebooksLocator);
            //_driver._wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(PricesNotebooksLocator));
        }

        public double[] GetPrices()
        {
            _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
            IList<IWebElement> pricesList = _driver.FindAllElementsWithWaiting(PricesNotebooksLocator);

            double[] pricesArray = new double[pricesList.Count];
            int i = 0;

            foreach (var item in pricesList)
            {
                string processedItem = item.GetAttribute("innerHTML").Replace("&nbsp;", "").Replace("р.","").Replace(',', '.');
                pricesArray[i++] = Convert.ToDouble(processedItem);
            }
            return pricesArray;
        }

        public string ConvertToStringPriceWithFormat(double price)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)
            CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 0;
            return price.ToString("n", nfi);
        }

        internal int[] GetRatings()
        {
            _driver.WaitElement(By.CssSelector(".schema-products_processing"));
            _driver.WaitElement(By.CssSelector(".schema-filter-button__state_animated"));
            _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
            _driver.WaitWhileElementClassContainsText(SchemaFilterButtonLocator, "schema-filter-button__state_animated");
            IList<IWebElement> ratingList = _driver.FindAllElementsWithRaiting(RaitingItemsLocator);
            int[] ratings = new int[ratingList.Count];
            int i = 0;
            foreach (var item in ratingList)
            {
                string classname = item.GetAttribute("class");
                int pos = classname.IndexOf('_');
                ratings[i++] = Convert.ToInt32(classname.Substring(pos+1, classname.Length - (pos +1)));
            }
                return ratings;
        }
    }
}
