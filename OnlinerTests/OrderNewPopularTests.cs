using NUnit.Framework;
using OnlinerTests.Pages;
using System.Collections.Generic;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class OrderNewPopularTests : OnlinerTestsSetup
    {
        [Test]
        public void OrderNewTest()
        {
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(OrderPageOnliner.OrderType.Newest);
            IList<string> fullNamesApi = notebooksPage.GetFullNames("https://catalog.api.onliner.by/search/notebook?group=0&order=date:desc");
            IList<string> fullnamesDriver = notebooksPage.GetFullNames(notebooksPage.FullNameItemsLocator);
            Assert.AreEqual(fullNamesApi, fullnamesDriver, "Fullname not match");
        }

        [Test]
        public void OrderPopularTest()
        {
            var orderPage = new OrderPageOnliner(_webDriver);
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            orderPage.SelectOrder(OrderPageOnliner.OrderType.Popular);
            IList<string> fullNamesApi = notebooksPage.GetFullNames("https://catalog.api.onliner.by/search/notebook?group=1&order=rating:desc");
            IList<string> fullnamesDriver = notebooksPage.GetFullNames(notebooksPage.FullNameItemsLocator);
            Assert.AreEqual(fullNamesApi, fullnamesDriver, "Fullname not match");
        }
    }
}