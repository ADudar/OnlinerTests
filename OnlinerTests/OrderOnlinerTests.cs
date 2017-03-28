using NUnit.Framework;
using OnlinerTests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class OrderOnlinerTests : OnlinerTestsSetup
    {
        [Test]
        public void OrderCheapTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            catalogPage.SelectOrder(catalogPage.CheapItemsLocator);
            log.Info("cheap items selected");

            int count = 0;
            double[] prices = new double[0];
            while (count < 4)
            {
                try
                {
                    prices = catalogPage.GetPrices();
                }
                catch (StaleElementReferenceException e)
                {
                    count++;
                }
                count = count + 4;
            }

            //double[] prices = catalogPage.GetPrices();
            for (int i = 0; i < prices.Length - 1; i++)
            {
                if (prices[i] > prices[i+1] )
                {
                    string msg = "Filter cheap is not applied";
                    log.Error(msg);
                    log.Fail(msg);
                    Assert.Fail(msg);
                    break;
                }
            }
            string message = "filter cheap is apply";
            log.Pass(message);
            Assert.Pass(message);
        }

        [Test]
        public void OrderExpensiveTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            catalogPage.SelectOrder(catalogPage.ExpensiveItemsLocator);
            log.Info("expensive items selected");
            //_webDriver._wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(catalogPage.ExpensiveItemsLocator));

            int count = 0;
            double[] prices = new double[0];
            while (count < 4)
            {
                try
                {
                    prices = catalogPage.GetPrices();
                }
                catch (StaleElementReferenceException e)
                {
                    count++;
                }
                count = count + 4;
            }

            //double[] prices = catalogPage.GetPrices();
            for (int i = 0; i < prices.Length - 1; i++)
            {
                if (prices[i] < prices[i + 1])
                {
                    string msg = "Filter expensive is not applied";
                    log.Error(msg);
                    log.Fail(msg);
                    Assert.Fail(msg);
                    break;
                }
            }
            string message = "filter expensive is apply";
            log.Pass(message);
            Assert.Pass(message);
        }
    }
}
