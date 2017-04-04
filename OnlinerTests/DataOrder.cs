using OnlinerTests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinerTests
{
    public class DataOrder
    {
        static object[] orderDataNotebook = {
        new object[] { OrderComponent.OrderType.Newest,  "https://catalog.api.onliner.by/search/notebook?group=0&order=date:desc" },
        new object[] { OrderComponent.OrderType.Popular, "https://catalog.api.onliner.by/search/notebook?group=1&order=rating:desc" }
        };

        static object[] orderDataTV = {
        new object[] { OrderComponent.OrderType.Newest,  "https://catalog.api.onliner.by/search/tv?group=0&order=date:desc" },
        new object[] { OrderComponent.OrderType.Popular, "https://catalog.api.onliner.by/search/tv?group=1&order=rating:desc" }
        };

        static object[] orderDataRefrigerator = {
        new object[] { OrderComponent.OrderType.Newest,  "https://catalog.api.onliner.by/search/refrigerator?order=date:desc" },
        new object[] { OrderComponent.OrderType.Popular, "https://catalog.api.onliner.by/search/refrigerator?order=rating:desc" }
        };
    }

}
