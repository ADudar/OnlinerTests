using OpenQA.Selenium;
using System.Collections.Generic;
using System;
using System.Linq;

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
        public By SchemaOrderOpenLocator { get; set; } = By.CssSelector(".schema-order_opened");
        public By StateSpanLocator { get; set; } = By.CssSelector(".schema-filter-control__switcher-state+span");
        #endregion

        public By GetLocatorForOrderType(OrderType orderType)
        {
            var orderTypeLocator = ".schema-order__item:nth-of-type({0})";
            switch (orderType)
            {
                case OrderType.Popular:
                    return By.CssSelector(string.Format(orderTypeLocator, (int)OrderType.Popular));
                case OrderType.Cheap:
                    return By.CssSelector(string.Format(orderTypeLocator, (int)OrderType.Cheap));
                case OrderType.Expensive:
                    return By.CssSelector(string.Format(orderTypeLocator, (int)OrderType.Expensive));
                case OrderType.Newest:
                    return By.CssSelector(string.Format(orderTypeLocator, (int)OrderType.Newest));
                case OrderType.Raiting:
                    return By.CssSelector(string.Format(orderTypeLocator, (int)OrderType.Raiting));
                default:
                    throw new Exception($"Unknown order type: {orderType}");
            }
        }

        public By GetLocatorForOrderState(OrderState orderState)
        {
            var orderStateLocator = "input[value={0}]+span";
            switch (orderState)
            {
                case OrderState.All:
                    return By.CssSelector(string.Format(orderStateLocator, OrderState.All.ToString().ToLower()));
                case OrderState.New:
                    return By.CssSelector(string.Format(orderStateLocator, OrderState.New.ToString().ToLower()));
                case OrderState.Second:
                    return By.CssSelector(string.Format(orderStateLocator, OrderState.Second.ToString().ToLower()));
                default:
                    throw new Exception($"Unknown order state: {orderState}");
            }
        }

        internal void SelectState(OrderState orderState)
        {
            if (orderState != GetSelectedState())
            {
                _driver.Click(GetLocatorForOrderState(orderState));
            }
        }

        public OrderState GetSelectedState()
        {
            Dictionary<By, OrderState> orderStates = new Dictionary<By, OrderState>()
            {
                {GetLocatorForOrderState(OrderState.All), OrderState.All},
                {GetLocatorForOrderState(OrderState.New), OrderState.New},
                {GetLocatorForOrderState(OrderState.Second), OrderState.Second}
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
                        _driver.Click(GetLocatorForOrderType(orderType));
            }
        }

        public OrderType GetSelectedOrder()
        {
            Dictionary<By, OrderType> orderTypes = new Dictionary<By, OrderType>()
            {
                {GetLocatorForOrderType(OrderType.Popular), OrderType.Popular },
                {GetLocatorForOrderType(OrderType.Cheap), OrderType.Cheap},
                {GetLocatorForOrderType(OrderType.Expensive), OrderType.Expensive},
                {GetLocatorForOrderType(OrderType.Newest), OrderType.Newest},
                {GetLocatorForOrderType(OrderType.Raiting), OrderType.Raiting},
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
