using NUnit.Framework;
using OnlinerTests.Pages;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterPricesNotebooksTests : TestsSetup
    {
        [TestCase(10000)]
        [Description("")]
        public void FilterMinPriceTest(double price)
        {
            //log.Pass(TestContext.CurrentContext.Test.Properties["Description"].ToString());
            var filterPage = new FilterComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/notebook");
            filterPage.SetMinPrice(price);
            string expectedStringPrice = filterPage.ConvertToStringPriceWithFormat(price);
            var actualValue = _webDriver.GetText(filterPage.FilterPriceLabelLocator);
            Assert.AreEqual("от " + expectedStringPrice, actualValue, "Error, filter label not set");
            log.Pass("filter label set success");
            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < price)
                {
                    Assert.Fail("founded items with less price, value: " + item);
                    break;
                }
            }
            string message = "all items have greater than min price";
            log.Pass(message);
        }

        [TestCase(300, 400)]
        public void FilterMinMaxPriceTest(double minPrice, double maxPrice)
        {
            var filterPage = new FilterComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/notebook");
            filterPage.SetMinMaxPrice(minPrice, maxPrice);
            string stringMinPrice = filterPage.ConvertToStringPriceWithFormat(minPrice);
            string stringMaxPrice = filterPage.ConvertToStringPriceWithFormat(maxPrice);
            string expectedStringPrice = stringMinPrice + " — " + stringMaxPrice;
            //Assert.AreEqual(expectedStringPrice, _webDriver.GetText(filterPage.FilterPriceLabelLocator), "Error, filter label not set");
            //log.Pass("filter label set success");
            string priceFromWebdriver = _webDriver.GetText(filterPage.FilterPriceLabelLocator);
            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < minPrice || item > maxPrice)
                {
                    Assert.Fail("founded items with less or great price");
                    break;
                }
            }
            string message = "all items greater than " + stringMinPrice + " and less than " + stringMaxPrice;
            log.Pass(message);
        }

        [TestCase(400)]
        public void FilterMaxPriceTest(double price)
        {
            var filterPage = new FilterComponent(_webDriver);
            var catalogPage= new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/notebook");
            filterPage.SetMaxPrice(price);
            string expectedStringPrice = filterPage.ConvertToStringPriceWithFormat(price);
            Assert.AreEqual("до " + expectedStringPrice, _webDriver.GetText(filterPage.FilterPriceLabelLocator), "Error, filter not set");
            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item > price)
                {
                    Assert.Fail("founded items with greater price");
                    break;
                }
            }
            string message = "all items have less than max price";
            log.Pass(message);
        }
    }
}
