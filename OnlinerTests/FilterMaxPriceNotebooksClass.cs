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
    public class FilterMaxPriceNotebooksClass : FilterPricesOnlinerTestSetUp
    {
        [TestCase(350)]
        public void FilterMaxPriceNotebooksTest(double price)
        {
            var dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = this.GetType().ToString() + ".html";
            var htmlReporter = new ExtentHtmlReporter(dir + fileName);
            _extent.AttachReporter(htmlReporter);

            var maxPriceTest = _extent.CreateTest("max price test");

            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            maxPriceTest.Info("navigate to notebooks page success");
            try
            {
                catalogPage.SetMaxPriceNotebooks(price);
            }
            catch
            {
                string msg = "error to set max price filter";
                log.Error(msg);
                Assert.Fail(msg);
                maxPriceTest.Fail(msg);
            }

            maxPriceTest.Debug("filter max price set to " + price);
            log.Debug("filter max price set to " + price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);

            try
            {
                Assert.AreEqual("до " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter not set");
                maxPriceTest.Pass("filter max price set success");

            }
            catch
            {
                string msg = "error to set max price filter";
                log.Error("filter max price not set");
                Assert.Fail(msg);
                maxPriceTest.Fail(msg);
            }

            log.Debug("filter max price set success");

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                if (item > price)
                {
                    string msg = "founded items with greater price";
                    log.Error(msg);
                    Assert.Fail(msg);
                    maxPriceTest.Fail(msg);
                    break;
                }
            }
            string message = "all items have less than max price";
            maxPriceTest.Pass(message);
            Assert.Pass(message);
        }
    }
}
