using NUnit.Framework;
using OnlinerTests.Pages;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class OrderCheapExpensiveRatingTests : OnlinerTestsSetup
    {
        [Test]
        public void OrderCheapTest()
        {
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(OrderPageOnliner.OrderType.Cheap);
            double[] prices = notebooksPage.GetPrices();
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
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(OrderPageOnliner.OrderType.Expensive);
            double[] prices = notebooksPage.GetPrices();
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
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(OrderPageOnliner.OrderType.Raiting);
            int[] ratings = notebooksPage.GetRatings();
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
