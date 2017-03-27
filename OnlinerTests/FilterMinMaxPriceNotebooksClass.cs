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
            var minMaxPriceTest = _extent.CreateTest("min-max price test");

            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            minMaxPriceTest.Info("navigate to notebooks page success");

            try
            {
                catalogPage.SetMinPriceNotebooks(minPrice);
                catalogPage.SetMaxPriceNotebooks(maxPrice);
            }

            catch
            {
                string msg = "error to set min or max price filter";
                log.Error(msg);
                Assert.Fail(msg);
                minMaxPriceTest.Fail(msg);
            }

            log.Debug("filter min price set to " + minPrice);
            log.Debug("filter max price set to " + maxPrice);
            minMaxPriceTest.Debug("filter min price set to " + minPrice);
            minMaxPriceTest.Debug("filter max price set to " + maxPrice);

            string stringMinPrice = catalogPage.ConvertToStringPriceWithFormat(minPrice);
            string stringMaxPrice = catalogPage.ConvertToStringPriceWithFormat(maxPrice);

            string expectedStringPrice = stringMinPrice + " — " + stringMaxPrice;
            string priceFromWebdriver = _webDriver.GetText(catalogPage.FilterPriceLocator);

            //int res = priceFromWebdriver.CompareTo(expectedStringPrice);
            //Assert.IsTrue(res == 0);

            log.Debug("filter max price set success");

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item < minPrice || item > maxPrice)
                {
                    string msg = "founded items with less or great price";
                    log.Error(msg);
                    Assert.Fail(msg);
                    minMaxPriceTest.Fail(msg);
                    break;
                }
            }

            string message = "all items greater than " + stringMinPrice + " and less than " + stringMaxPrice;
            minMaxPriceTest.Pass(message);
            Assert.Pass(message);
        }
    }
}
