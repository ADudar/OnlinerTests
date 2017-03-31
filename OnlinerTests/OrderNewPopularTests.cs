using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OnlinerTests.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class OrderNewPopularTests : OnlinerTestsSetup
    {
        [Test]
        public void OrderNewTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SelectOrder(CatalogPageOnliner.OrderType.Newest);
            IList<string> fullNamesApi = catalogPage.GetNotebooksFullNames("https://catalog.api.onliner.by/search/notebook?group=0&order=date:desc");
            IList<string> fullnamesDriver = catalogPage.GetNotebooksFullNames(catalogPage.FullNameItemsLocator);
            Assert.AreEqual(fullNamesApi, fullnamesDriver, "Fullname not match");
        }

        [Test]
        public void OrderPopularTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            catalogPage.SelectOrder(CatalogPageOnliner.OrderType.Popular);
            IList<string> fullNamesApi = catalogPage.GetNotebooksFullNames("https://catalog.api.onliner.by/search/notebook?group=1&order=rating:desc");
            IList<string> fullnamesDriver = catalogPage.GetNotebooksFullNames(catalogPage.FullNameItemsLocator);
                Assert.AreEqual(fullNamesApi, fullnamesDriver, "Fullname not match");
        }
    }
}