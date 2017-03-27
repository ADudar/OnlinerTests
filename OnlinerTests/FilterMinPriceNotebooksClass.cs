using System;
using NUnit.Framework;
using OnlinerTests.Pages;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterMinPriceNotebooksClass : FilterPricesOnlinerTestSetUp
    {
        [TestCase(500)]
        public void FilterMinPriceNotebooksTest(double price)
        {
            var dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = this.GetType().ToString() + ".html";
            var htmlReporter = new ExtentHtmlReporter(dir + fileName);
            _extent.AttachReporter(htmlReporter);

            var log = _extent.CreateTest("min price test");

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
                Assert.Fail(msg);
                log.Fail(msg);
            }

            log.Debug("filter min price set to " + price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);

            try
            {
                Assert.AreEqual("от " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter not set");
                log.Pass("filter min price set success");
            }
            catch
            {
                string msg= "filter min price not set";
                log.Error(msg);
                log.Fail(msg);
                Assert.Fail(msg);
            }

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < price)
                {
                    string msg = "founded items with less price";
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
    }

}
