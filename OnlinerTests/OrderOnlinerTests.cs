using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OnlinerTests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class OrderCheapExpensiveRatingTests : OnlinerTestsSetup
    {
        [Test]
        public void OrderCheapTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SelectOrder(CatalogPageOnliner.OrderType.Cheap);
            double[] prices = catalogPage.GetPrices();
            for (int i = 0; i < prices.Length - 1; i++)
            {
                if (prices[i] > prices[i + 1])
                {
                    Assert.Fail("Filter cheap is not applied");
                    break;
                }
            }
        }

        [Test]
        public void OrderExpensiveTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SelectOrder(CatalogPageOnliner.OrderType.Expensive);
            double[] prices = catalogPage.GetPrices();
            for (int i = 0; i < prices.Length - 1; i++)
            {
                if (prices[i] < prices[i + 1])
                {
                    Assert.Fail("Filter expensive is not applied");
                    break;
                }
            }
        }

        [Test]
        public void OrderRatingTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SelectOrder(CatalogPageOnliner.OrderType.Raiting);
            int[] ratings = catalogPage.GetRatings();
            for (int i = 0; i < ratings.Length - 1; i++)
            {
                if (ratings[i] < ratings[i + 1])
                {
                    Assert.Fail("Rating filter is not applied");
                    break;
                }
            }
        }
    }
}
