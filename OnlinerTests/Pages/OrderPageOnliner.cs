using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinerTests.Pages
{
    public class OrderPageOnliner
    {
        private WebDriver _driver;

        public OrderPageOnliner(WebDriver driver)
        {
            _driver = driver;
        }

        public By OrderProductLocator { get; set; } = By.CssSelector(".schema-order__link");

        public By PopularSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(1)");

        public By CheapSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(2)");

        public By ExpensiveSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(3)");

        public By NewestSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(4)");

        public By RaitingSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(5)");

        public void SelectOrder(OrderType orderType)
        {
            if (orderType != GetSelectedOrder())
            {
                _driver.Click(OrderProductLocator);
                switch (orderType)
                {
                    case OrderType.Popular:
                        _driver.Click(PopularSelectLocator);
                        break;
                    case OrderType.Cheap:
                        _driver.Click(CheapSelectLocator);
                        break;
                    case OrderType.Expensive:
                        _driver.Click(ExpensiveSelectLocator);
                        break;
                    case OrderType.Newest:
                        _driver.Click(NewestSelectLocator);
                        break;
                    case OrderType.Raiting:
                        _driver.Click(RaitingSelectLocator);
                        break;
                    default:
                        break;
                }
            }
        }

        public OrderType GetSelectedOrder()
        {
            Dictionary<By, OrderType> orderTypes = new Dictionary<By, OrderType>()
            {
                {PopularSelectLocator, OrderType.Popular },
                {CheapSelectLocator, OrderType.Cheap},
                {ExpensiveSelectLocator, OrderType.Expensive},
                {NewestSelectLocator, OrderType.Newest},
                {RaitingSelectLocator, OrderType.Raiting},
            };

            foreach (var item in orderTypes)
            {
                if (_driver.CheckContainsClass(item.Key, "schema-order__item_active"))
                {
                    return item.Value;
                }
            }
            return OrderType.Popular;
        }

        public enum OrderType
        {
            Popular, Cheap, Expensive, Newest, Raiting
        }
    }
}
