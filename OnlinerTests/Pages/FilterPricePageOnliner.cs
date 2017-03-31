﻿using OpenQA.Selenium;
using System.Globalization;

namespace OnlinerTests.Pages
{
    public class FilterPricePageOnliner
    {
        private WebDriver _driver;

        public FilterPricePageOnliner(WebDriver driver)
        {
            _driver = driver;
        }


        public By InputPriceFromLocator { get; set; } = By.XPath("//input[contains(@class, 'schema-filter__number-input_price') and contains(@data-bind, 'value: facet.value.from')]");

        public By InputPriceToLocator { get; set; } = By.XPath("//input[contains(@class, 'schema-filter__number-input_price') and contains(@data-bind, 'value: facet.value.to')]");

        public By FilterPriceLabelLocator { get; set; } = By.ClassName("schema-tags__text");

        //public By PricesItemsLocator { get; set; } = By.XPath("//a[contains(@class, 'schema-product__price-value_primary')]/span[contains(@data-bind,'minPrice')]");



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
