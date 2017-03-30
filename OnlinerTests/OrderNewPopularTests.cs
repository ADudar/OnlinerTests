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
        public void OrderNewItemsTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            log.Info("navigate to notebooks page success");
            catalogPage.SelectOrder(catalogPage.NewSelectLocator);
            log.Info("new items selected");
            IList<string> fullNamesApi = catalogPage.GetNotebooksFullNames("https://catalog.api.onliner.by/search/notebook?group=0&order=date:desc");
            log.Info("get fullnames from api success");
            IList<string> fullnamesDriver = catalogPage.GetNotebooksFullNames(catalogPage.FullNameItemsLocator);
            log.Info("get fullnames from webDriver success");

            string msg = "Fullname not match";
            try
            {
                Assert.AreEqual(fullNamesApi, fullnamesDriver, msg);
                log.Pass("All fullnames match");
            }
            catch (Exception)
            {
                log.Fail(msg);
            }
        }

        [Test]
        public void OrderPopularItemsTest()
        {
            var catalogPage = new CatalogPageOnliner(_webDriver);
            catalogPage.Open();
            log.Info("navigate to notebooks page success");
            catalogPage.SelectOrder(catalogPage.PopularSelectLocator);
            log.Info("popular items selected");
            IList<string> fullNamesApi = catalogPage.GetNotebooksFullNames("https://catalog.api.onliner.by/search/notebook?group=1&order=rating:desc");
            log.Info("get fullnames from api success");
            IList<string> fullnamesDriver = catalogPage.GetNotebooksFullNames(catalogPage.FullNameItemsLocator);
            log.Info("get fullnames from webDriver success");

            string msg = "Fullname not match";
            try
            {
                Assert.AreEqual(fullNamesApi, fullnamesDriver, msg);
                log.Pass("All fullnames match");
            }
            catch (Exception)
            {
                log.Fail(msg);
            }

        }
    }
}