using NUnit.Framework;
using OnlinerTests.Pages;
using System.Collections.Generic;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class OrderNewPopularTests : OnlinerTestsSetup
    {
        [TestCaseSource(typeof(OrderData), "orderData")]
        public void OrderTest(OrderPageOnliner.OrderType orderType, string url)
        {
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(orderType);
            IList<string> fullNamesApi = notebooksPage.GetFullNames(url);
            IList<string> fullnamesDriver = notebooksPage.GetFullNames(notebooksPage.FullNameItemsLocator);
            Assert.AreEqual(fullNamesApi, fullnamesDriver, "Fullname not match");
            string message = $"Success apply { orderType } filter";
            log.Pass(message);
        }
    }

    class OrderData
    {
        static object[] orderData = {
        new object[] { OrderPageOnliner.OrderType.Newest,  "https://catalog.api.onliner.by/search/notebook?group=0&order=date:desc" },
        new object[] { OrderPageOnliner.OrderType.Popular, "https://catalog.api.onliner.by/search/notebook?group=1&order=rating:desc" }
        };

    }
}