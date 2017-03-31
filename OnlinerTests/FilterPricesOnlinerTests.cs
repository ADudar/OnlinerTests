using NUnit.Framework;
using OnlinerTests.Pages;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterPricesOnlinerTests : OnlinerTestsSetup
    {
        [TestCase(10000)]
        public void FilterMinPriceTest(double price)
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SetMinPrice(price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);
            Assert.AreEqual("от " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter label not set");
            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < price)
                {
                    Assert.Fail("founded items with less price, value: " + item);
                    break;
                }
            }
            Assert.Pass("all items have greater than min price");
        }

        [TestCase(300, 400)]
        public void FilterMinMaxPriceTest(double minPrice, double maxPrice)
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SetMinMaxPrice(minPrice, maxPrice);
            string stringMinPrice = catalogPage.ConvertToStringPriceWithFormat(minPrice);
            string stringMaxPrice = catalogPage.ConvertToStringPriceWithFormat(maxPrice);
            string expectedStringPrice = stringMinPrice + " — " + stringMaxPrice;
            Assert.AreEqual(expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter label not set");
            string priceFromWebdriver = _webDriver.GetText(catalogPage.FilterPriceLocator);
            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < minPrice || item > maxPrice)
                {
                    Assert.Fail("founded items with less or great price");
                    break;
                }
            }
            Assert.Pass("all items greater than " + stringMinPrice + " and less than " + stringMaxPrice);
        }

        [TestCase(400)]
        public void FilterMaxPriceTest(double price)
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SetMaxPrice(price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);
            Assert.AreEqual("до " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter not set");
            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item > price)
                {
                    Assert.Fail("founded items with greater price");
                    break;
                }
            }
            Assert.Pass("all items have less than max price");
        }
    }
}
