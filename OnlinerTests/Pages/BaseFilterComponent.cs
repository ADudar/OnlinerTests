using OpenQA.Selenium;
using System.Globalization;

namespace OnlinerTests.Pages
{
    public class BaseFilterComponent
    {
        protected WebDriver _driver;

        public BaseFilterComponent(WebDriver driver)
        {
            _driver = driver;
        }

        public By FilterPriceLabelLocator { get; set; } = By.ClassName("schema-tags__text");

        public enum FilterPriceType
        {
            FilterPriceFrom,
            FilterPriceTo
        }

        public By GetInputPriceLocator(FilterPriceType type)
        {
            string filterLocator = "//input[contains(@class, 'schema-filter__number-input_price') and contains(@data-bind, 'value: facet.value.{0}')]";
            switch (type)
            {
                case FilterPriceType.FilterPriceFrom:
                    return By.XPath(string.Format(filterLocator, "from"));
                case FilterPriceType.FilterPriceTo:
                    return By.XPath(string.Format(filterLocator, "to"));
                default:
                    throw new System.Exception($"Unknown order state: {type}");
            }
        }

        public void SetMinPrice(double minPrice)
        {
            _driver.SendKeys(GetInputPriceLocator(FilterPriceType.FilterPriceFrom), minPrice.ToString());
        }

        public void SetMaxPrice(double maxPrice)
        {
            _driver.SendKeys(GetInputPriceLocator(FilterPriceType.FilterPriceTo), maxPrice.ToString());
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
