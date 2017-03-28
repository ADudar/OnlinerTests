using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Globalization;

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

        public By NotebookItemDivLocator { get; set; } = By.CssSelector(".schema-product__group");

        public By PricesNotebooksLocator { get; set; } = By.XPath("//a[contains(@class, 'schema-product__price-value_primary')]/span[contains(@data-bind,'minPrice')]");

        public By FilterPriceLocator { get; set; } = By.ClassName("schema-tags__text");

        public By LoadingProductLocator { get; set; } = By.CssSelector(".schema-products");

        public By OrderProductLocator { get; set; } = By.CssSelector(".schema-order__link");

        public By PopularItemsLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(1)");

        public By CheapItemsLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(2)");

        public By ExpensiveItemsLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(3)");

        public By NewItemsLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(4)");

        public By RaitingItemsLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(5)");

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
            _driver.FindAllElementsWithWaiting(PricesNotebooksLocator);
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
    }
}
