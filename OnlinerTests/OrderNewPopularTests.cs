using NUnit.Framework;
using OnlinerTests.Pages;
using OpenQA.Selenium;
using System;
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
            catalogPage.NavigateToNotebooksPage();
            log.Info("navigate to notebooks page success");
            catalogPage.SelectOrder(catalogPage.NewSelectLocator);
            log.Info("new items selected");
            
        }
    }


    [TestFixture]
    public class WebRequestTest
    {
        [Test]
        public void GetNewItemsTest()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://catalog.api.onliner.by/search/notebook?group=0&order=date:desc");
            request.Method = "GET";
            request.Accept = "text/javascript";
            request.Accept = "application/json";
            request.Headers.Add("Accept-Encoding", "gzip");


        }
    }
}
