using System;
using NUnit.Framework;
using OnlinerTests.Pages;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterMinPriceNotebooksClass : FilterPricesOnlinerTestSetUp
    {
        [TestCase(500)]
        public void FilterMinPriceNotebooksTest(double price)
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
                string message = "error to set min price filter";
                log.Error(message);
                Assert.Fail(message);
            }

            log.Debug("filter min price set to " + price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);

            try
            {
                Assert.AreEqual("от " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter not set");
            }
            catch
            {
                string message = "filter min price not set";
                log.Error(message);
                Assert.Fail(message);
            }
            log.Debug("filter min price set success");

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                try
                {
                    Assert.GreaterOrEqual(item, price);
                }
                catch (Exception)
                {
                    string message = "founded items with less price";
                    log.Error(message);
                    Assert.Fail(message);
                }
            }
        }
    }
}
