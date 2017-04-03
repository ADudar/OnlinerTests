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
            notebooksPage.WaitAjaxResponse();
            double[] items = notebooksPage.GetPrices();
            for (int i = 0; i < items.Length - 1; i++)
            {
                if (items[i] > items[i + 1])
                {
                    Assert.Fail(" Cheap filter is not applied");
                    break;
                }
            }
            string message = "Success apply Cheap filter";
            log.Pass(message);
        }

        [Test]
        public void OrderExpensiveTest()
        {
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(OrderPageOnliner.OrderType.Expensive);
            notebooksPage.WaitAjaxResponse();
            double[] items = notebooksPage.GetPrices();
            for (int i = 0; i < items.Length - 1; i++)
            {
                if (items[i] < items[i + 1])
                {
                    Assert.Fail(" Expensive filter is not applied");
                    break;
                }
            }
            string message = "Success apply Expensive filter";
            log.Pass(message);
        }

        [Test]
        public void OrderRaitingTest()
        {
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(OrderPageOnliner.OrderType.Raiting);
            notebooksPage.WaitAjaxResponse();
            double[] items = notebooksPage.GetRatings();
            for (int i = 0; i < items.Length - 1; i++)
            {
                if (items[i] < items[i + 1])
                {
                    Assert.Fail(" Raiting filter is not applied");
                    break;
                }
            }
            string message = "Success apply Raiting filter";
            log.Pass(message);
        }
    }

    //class DataOrder
    //{
    //    static Func<double[]> GetRatings = CatalogPageOnliner.GetRatings;
    //    static Func<double[]> GetPrices  = CatalogPageOnliner.GetPrices;
    //    static Func<double, double, bool> CompareLess = (x1, x2) => x1 < x2;
    //    static Func<double, double, bool> CompareGreater = (x2, x1) => x1 < x2;

    //    static object[] orderData = {
    //    new object[] { OrderPageOnliner.OrderType.Raiting,  GetRatings, CompareLess },
    //    new object[] { OrderPageOnliner.OrderType.Expensive,  GetPrices, CompareLess },
    //    new object[] { OrderPageOnliner.OrderType.Cheap,  GetPrices, CompareGreater }
    //    };
    //}
}
