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

    public class FilterMinMaxPriceNotebooksClass : FilterPricesOnlinerTestSetUp
    {


        [TestCase(100, 300)]
        public void FilterMinMaxPriceNotebooksTest(double minPrice, double maxPrice)
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
                string message = "error to set min or max price filter";
                log.Error(message);
                Assert.Fail(message);
            }

            log.Debug("filter max price set to " + minPrice);
            log.Debug("filter max price set to " + maxPrice);
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
                try
                {
                    Assert.LessOrEqual(item, maxPrice);
                    Assert.GreaterOrEqual(item, minPrice);
                }
                catch
                {
                    string message = "founded items with greater or less price";
                    log.Error(message);
                    Assert.Fail(message);
                }
            }
        }
    }
}
