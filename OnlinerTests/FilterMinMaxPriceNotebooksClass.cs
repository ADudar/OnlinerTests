using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using OnlinerTests.Pages;
using log4net.Config;
using log4net;
using System.Reflection;
using AventStack.ExtentReports.Reporter;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterMinMaxPriceNotebooksClass : FilterPricesOnlinerTestSetUp
    {
        [TestCase(100, 300)]
        public void FilterMinMaxPriceNotebooksTest(double minPrice, double maxPrice)
        {
            var dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = this.GetType().ToString() + ".html";
            var htmlReporter = new ExtentHtmlReporter(dir + fileName);
            _extent.AttachReporter(htmlReporter);
            var log = _extent.CreateTest("min-max price test");

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

            log.Debug("filter min price set to " + minPrice);
            log.Debug("filter max price set to " + maxPrice);

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
    }
}
