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
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            try
            {
                catalogPage.SetMinPriceNotebooks(price);
            }
            catch
            {
                string msg = "error to set min price filter";
                log.Error(msg);
                log.Fail(msg);
                Assert.Fail(msg);
            }

            log.Info("filter min price set to " + price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);

            try
            {
                Assert.AreEqual("от " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter not set");
                log.Pass("filter min price set success");
            }
            catch
            {
                string msg = "filter min price not set";
                log.Error(msg);
                log.Fail(msg);
                Assert.Fail(msg);
            }

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < price)
                {
                    string msg = "founded items with less price, value: " + item;
                    log.Error(msg);
                    Assert.Fail(msg);
                    log.Fail(msg);
                    break;
                }
            }
            string message = "all items have greater than min price";
            log.Pass(message);
            Assert.Pass(message);
        }

        [TestCase(100, 300)]
        public void FilterMinMaxPriceTest(double minPrice, double maxPrice)
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            try
            {
                catalogPage.SetMinPriceNotebooks(minPrice);
                catalogPage.SetMaxPriceNotebooks(maxPrice);
            }
            catch
            {
                string msg = "error to set min or max price filter";
                log.Error(msg);
                log.Fail(msg);
                Assert.Fail(msg);
            }

            log.Info("filter min price set to " + minPrice);
            log.Info("filter max price set to " + maxPrice);

            string stringMinPrice = catalogPage.ConvertToStringPriceWithFormat(minPrice);
            string stringMaxPrice = catalogPage.ConvertToStringPriceWithFormat(maxPrice);
            string expectedStringPrice = stringMinPrice + " — " + stringMaxPrice;
            string priceFromWebdriver = _webDriver.GetText(catalogPage.FilterPriceLocator);

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < minPrice || item > maxPrice)
                {
                    string msg = "founded items with less or great price";
                    log.Error(msg);
                    log.Fail(msg);
                    Assert.Fail(msg);
                    break;
                }
            }

            string message = "all items greater than " + stringMinPrice + " and less than " + stringMaxPrice;
            log.Pass(message);
            Assert.Pass(message);
        }

        [TestCase(350)]
        public void FilterMaxPriceTest(double price)
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            try
            {
                catalogPage.SetMaxPriceNotebooks(price);
            }
            catch
            {
                string msg = "error to set max price filter";
                log.Error(msg);
                log.Fail(msg);
                Assert.Fail(msg);
            }

            log.Info("filter max price set to " + price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);

            try
            {
                Assert.AreEqual("до " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter not set");
                log.Pass("filter max price set success");

            }
            catch
            {
                string msg = "error to set max price filter";
                log.Error(msg);
                log.Fail(msg);
                Assert.Fail(msg);
            }

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item > price)
                {
                    string msg = "founded items with greater price";
                    log.Error(msg);
                    log.Fail(msg);
                    Assert.Fail(msg);
                    break;
                }
            }
            string message = "all items have less than max price";
            log.Pass(message);
            Assert.Pass(message);
        }
    }

}
