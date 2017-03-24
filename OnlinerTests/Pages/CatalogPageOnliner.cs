using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void NavigateToNotebooksPage()
        {
            _driver.Click(NotebooksLinkLocator);
        }

        public void SetMinPriceNotebooks(double minPrice)
        {
            _driver.WaitElement(InputPriceFromLocator);
            _driver.SendKeys(InputPriceFromLocator, minPrice.ToString());
            _driver.WaitElement(NotebookItemDivLocator);
        }

        public void SetMaxPriceNotebooks(double maxPrice)
        {
            _driver.WaitElement(InputPriceToLocator);
            _driver.SendKeys(InputPriceToLocator, maxPrice.ToString());
            _driver.WaitElement(NotebookItemDivLocator);
        }

        public double[] GetPrices()
        {
            _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
            IList<IWebElement> pricesList = _driver.FindAllElementsWithWaiting(PricesNotebooksLocator);
            double[] pricesArray = new double[pricesList.Count];
            int i = 0;

            foreach (var item in pricesList)
            {
                int startTrim = 0;
                int endTrimPosition = item.GetAttribute("innerHTML").IndexOf('&');
                pricesArray[i++] = Convert.ToDouble(
                    item.GetAttribute("innerHTML")
                    .Substring(startTrim, endTrimPosition)
                    .Replace(',', '.')
                    );
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
