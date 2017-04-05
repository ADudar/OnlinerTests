using NUnit.Framework;
using OnlinerTests.Pages;
using System.Collections.Generic;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class OrderNotebooksTests : TestsSetup
    {
        [Test]
        public void OrderCheapNotebooksTest()
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/notebook");
            orderPage.SelectOrder(OrderComponent.OrderType.Cheap);
            catalogPage.WaitPageLoad();
            orderPage.SelectState(OrderComponent.OrderState.New);
            catalogPage.WaitPageLoad();
            double[] items = catalogPage.GetPrices();
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
        public void OrderExpensiveNotebooksTest()
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/notebook");
            orderPage.SelectOrder(OrderComponent.OrderType.Expensive);
            catalogPage.WaitPageLoad();
            orderPage.SelectState(OrderComponent.OrderState.New);
            catalogPage.WaitPageLoad();
            double[] items = catalogPage.GetPrices();
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
        public void OrderRaitingNotebooksTest()
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/notebook");
            orderPage.SelectOrder(OrderComponent.OrderType.Raiting);
            orderPage.SelectState(OrderComponent.OrderState.New);
            catalogPage.WaitPageLoad();
            double[] items = catalogPage.GetRatings();
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

        [TestCaseSource(typeof(DataOrder), "orderDataNotebook")]
        public void OrderNotebooksTest(OrderComponent.OrderType orderType, string url)
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            IList<string> fullNamesApi = catalogPage.GetFullNamesFromUrl(url);
            catalogPage.Open("https://catalog.onliner.by/notebook");
            orderPage.SelectOrder(orderType);
            catalogPage.WaitPageLoad();
            orderPage.SelectState(OrderComponent.OrderState.New);
            catalogPage.WaitPageLoad();
            IList<string> fullnamesDriver = catalogPage.GetFullNamesFromLocator(catalogPage.FullNameItemsLocator);
            Assert.AreEqual(fullNamesApi, fullnamesDriver, "Fullname not match");
            string message = $"Success apply { orderType } filter";
            log.Pass(message);
        }
    }
}
