using OpenQA.Selenium;
using System.Collections.Generic;
using System;

namespace OnlinerTests.Pages
{
    public class OrderComponent
    {
        private WebDriver _driver;

        public enum OrderType
        {
            Popular = 1,
            Cheap,
            Expensive,
            Newest,
            Raiting
        }

        public enum OrderState
        {
            All, New, Second
        }

        public OrderComponent(WebDriver driver)
        {
            _driver = driver;
        }

        #region locators
        public By OrderProductLocator { get; set; } = By.CssSelector(".schema-order__link");
        public By PopularSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(1)");
        public By CheapSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(2)");
        public By ExpensiveSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(3)");
        public By NewestSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(4)");
        public By RaitingSelectLocator { get; set; } = By.CssSelector(".schema-order__item:nth-of-type(5)");
        public By SchemaOrderOpenLocator { get; set; } = By.CssSelector(".schema-order_opened");
        public By StateAllLocator { get; set; } = By.CssSelector("input[value=all]+span");
        public By StateNewLocator { get; set; } = By.CssSelector("input[value=new]+span");
        public By StateSecondLocator { get; set; } = By.CssSelector("input[value=second]+span");
        public By StateSpanLocator { get; set; } = By.CssSelector(".schema-filter-control__switcher-state+span");
        #endregion

        //public By GetLocatorForOrderType(OrderType orderType)
        //{
        //    Console.WriteLine("{0}, {1}", 10, 20);
        //    var format = ".schema-order__item:nth-of-type({0})";
        //    switch (orderType)
        //    {
        //        case OrderType.Popular:
        //            return By.CssSelector(string.Format(format, 0));
        //        case OrderType.Cheap:
        //            return By.CssSelector(string.Format(format, 1));
        //            break;
        //        case OrderType.Expensive:
        //            break;
        //        case OrderType.Newest:
        //            break;
        //        case OrderType.Raiting:
        //            break;
        //        default:
        //            throw new Exception($"Unknown order type: {orderType}");
        //    }
        //    return By.CssSelector($);
        //}

        internal void SelectState(OrderState state)
        {
            if (state != GetSelectedState())
            {
                switch (state)
                {
                    case OrderState.All:
                        _driver.Click(StateAllLocator);
                        break;
                    case OrderState.New:
                        _driver.Click(StateNewLocator);
                        break;
                    case OrderState.Second:
                        _driver.Click(StateSecondLocator);
                        break;
                    default:
                        break;
                }
            }
        }

        public OrderState GetSelectedState()
        {
            Dictionary<By, OrderState> orderStates = new Dictionary<By, OrderState>()
            {
                {StateAllLocator, OrderState.All},
                {StateNewLocator, OrderState.New},
                {StateSecondLocator, OrderState.Second}
            };

            foreach (var item in orderStates)
            {
                var el = _driver.Driver.FindElement(item.Key);
                if (el.GetCssValue("background").Equals("#555"))
                {
                    return item.Value;
                }
            }
            return OrderState.All;
        }

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
    }
}
