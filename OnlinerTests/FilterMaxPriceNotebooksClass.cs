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

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterMaxPriceNotebooksClass : FilterPricesOnlinerTestSetUp
    {
        [TestCase(350)]
        public void FilterMaxPriceNotebooksTest(double price)
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
                log.Error("error to set max price filter");
            }

            log.Debug("filter max price set to " + price);
            string expectedStringPrice = catalogPage.ConvertToStringPriceWithFormat(price);

            try
            {
                Assert.AreEqual("до " + expectedStringPrice, _webDriver.GetText(catalogPage.FilterPriceLocator), "Error, filter not set");
            }
            catch
            {
                log.Error("filter max price not set");
                throw;
            }
            log.Debug("filter max price set success");

            double[] prices = catalogPage.GetPrices();
            foreach (var item in prices)
            {
                try
                {
                    Assert.LessOrEqual(item, price);
                }
                catch
                {
                    log.Error("founded items with greater price");
                    throw;
                }
            }
        }


    }
}
