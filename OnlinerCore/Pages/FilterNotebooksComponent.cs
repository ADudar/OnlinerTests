using OpenQA.Selenium;
using System;

namespace OnlinerCore.Pages
{
    public class FilterNotebooksComponent : BaseFilterComponent
    {

        public enum ProcessorType
        {
            AMD_A10,
            AMD_A6,
            Intel_Atom,
            Samsung,
            Tegra_K1,
            Intel_Core_i7,
        }

        public FilterNotebooksComponent(WebDriver driver) : base(driver) { }

        #region locators
        public By ProcessorSelectLocator { get; set; } = By.XPath("//span[contains(text(),'Процессор')]/ancestor::*/following-sibling::div/child::div[contains(@data-bind,'click')]");
        public By ProcessorFilterVisibleLocator { get; set; } = By.XPath("//span[contains(text(),'Процессор')]/ancestor::*/following-sibling::div/child::div[contains(@class,'schema-filter-popover__wrapper')]/div");
        #endregion

        public By GetLocatorForProcessorType(ProcessorType type)
        {
            var processorLocator = ".schema-filter-popover_visible input[value={0}]+span";
            switch (type)
            {
                case ProcessorType.AMD_A10:
                    return By.CssSelector(string.Format(processorLocator, "amda10"));
                case ProcessorType.AMD_A6:
                    return By.CssSelector(string.Format(processorLocator, "amda6"));
                case ProcessorType.Intel_Atom:
                    return By.CssSelector(string.Format(processorLocator, "intelatom"));
                case ProcessorType.Samsung:
                    return By.CssSelector(string.Format(processorLocator, "samsung"));
                case ProcessorType.Tegra_K1:
                    return By.CssSelector(string.Format(processorLocator, "tegrak1"));
                case ProcessorType.Intel_Core_i7:
                    return By.CssSelector(string.Format(processorLocator, "intelcorei7"));
                default:
                    throw new Exception($"Unknown order state: {type}");
            }
        }

        public void SelectProcessor(ProcessorType[] processorTypes)
        {
            if (!_driver.CheckContainsClass(ProcessorFilterVisibleLocator, "schema-filter-popover_visible"))
            {
                var selectButton = _driver.WaitElementClickable(ProcessorSelectLocator);
                _driver.Scroll(selectButton.Location.Y);
                _driver.Click(ProcessorSelectLocator);
            }
            foreach (var item in processorTypes)
            {
                if (!ProcessorIsChecked(item))
                {
                    _driver.Click(GetLocatorForProcessorType(item));
                }
            }
        }

        private bool ProcessorIsChecked(ProcessorType type)
        {
            var locator = GetLocatorForProcessorType(type).ToString();
            locator = locator.Substring(locator.IndexOf(' '), locator.Length - locator.IndexOf(' '));
            string script = $"return window.getComputedStyle(document.querySelector('{locator}'),':before').getPropertyValue('opacity')";
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver.Driver;
            string opacity = (string)js.ExecuteScript(script);
            if (opacity == "1") return true;
            return false;
        }

    }
}
