using NUnit.Framework;
using OnlinerCore.Pages;
using System.Collections.Generic;

namespace OnlinerTests.Tests.Order.Refrigerators
{
    [TestFixture]
    [Parallelizable]
    public class OrderRefrigeratorsTests : TestsSetup
    {
        [Test]
        public void OrderCheapRefrigeratorsTest()
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/refrigerator");
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
        public void OrderExpensiveRefrigeratorsTest()
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/refrigerator");
            orderPage.SelectOrder(OrderComponent.OrderType.Expensive);
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
        public void OrderRaitingRefrigeratorsTest()
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/refrigerator");
            orderPage.SelectOrder(OrderComponent.OrderType.Raiting);
            catalogPage.WaitPageLoad();
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


        [TestCaseSource("orderDataRefrigerator")]
        public void OrderRefrigeratorsTest(OrderComponent.OrderType orderType, string url)
        {
            var orderPage = new OrderComponent(_webDriver);
            var catalogPage = new ResultsComponent(_webDriver);
            catalogPage.Open("https://catalog.onliner.by/refrigerator");
            orderPage.SelectOrder(orderType);
            catalogPage.WaitPageLoad();
            orderPage.SelectState(OrderComponent.OrderState.New);
            catalogPage.WaitPageLoad();
            IList<string> fullNamesApi = catalogPage.GetFullNamesFromUrl(url);
            IList<string> fullnamesDriver = catalogPage.GetFullNamesFromLocator(catalogPage.FullNameItemsLocator);
            Assert.AreEqual(fullNamesApi, fullnamesDriver, "Fullname not match");
            string message = $"Success apply { orderType } filter";
            log.Pass(message);
        }

        static object[] orderDataRefrigerator = {
        new object[] { OrderComponent.OrderType.Newest,  "https://catalog.api.onliner.by/search/refrigerator?order=date:desc" },
        new object[] { OrderComponent.OrderType.Popular, "https://catalog.api.onliner.by/search/refrigerator?order=rating:desc" }
        };
    }
}
