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
            var filterPage = new FilterPricePageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            filterPage.SetMinPrice(price);
            string expectedStringPrice = filterPage.ConvertToStringPriceWithFormat(price);
            Assert.AreEqual("от " + expectedStringPrice, _webDriver.GetText(filterPage.FilterPriceLabelLocator), "Error, filter label not set");
            double[] prices = notebooksPage.GetPrices();
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
            var filterPage = new FilterPricePageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            filterPage.SetMinMaxPrice(minPrice, maxPrice);
            string stringMinPrice = filterPage.ConvertToStringPriceWithFormat(minPrice);
            string stringMaxPrice = filterPage.ConvertToStringPriceWithFormat(maxPrice);
            string expectedStringPrice = stringMinPrice + " — " + stringMaxPrice;
            Assert.AreEqual(expectedStringPrice, _webDriver.GetText(filterPage.FilterPriceLabelLocator), "Error, filter label not set");
            string priceFromWebdriver = _webDriver.GetText(filterPage.FilterPriceLabelLocator);
            double[] prices = notebooksPage.GetPrices();
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
            var filterPage = new FilterPricePageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            filterPage.SetMaxPrice(price);
            string expectedStringPrice = filterPage.ConvertToStringPriceWithFormat(price);
            Assert.AreEqual("до " + expectedStringPrice, _webDriver.GetText(filterPage.FilterPriceLabelLocator), "Error, filter not set");
            double[] prices = notebooksPage.GetPrices();
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
