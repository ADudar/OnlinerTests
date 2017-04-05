using OpenQA.Selenium;
using System;

namespace OnlinerTests.Pages
{
    public class FilterNotebooksComponent : FilterComponent
    {

        public enum ProcessorType
        {
            None = 0,
            AMD_A10 = 1,
            AMD_A6 = 2,
            Intel_Atom = 4,
            Samsung = 8,
            Tegra_K1 = 16,
            Intel_Core_i7 = 32,
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
                //case ProcessorType.None:
                //    return;
                case ProcessorType.AMD_A10:
                    return By.CssSelector(string.Format(processorLocator, ProcessorType.AMD_A10.ToString().ToLower().Replace("_", "")));
                case ProcessorType.AMD_A6:
                    return By.CssSelector(string.Format(processorLocator, ProcessorType.AMD_A6.ToString().ToLower().Replace("_", "")));
                case ProcessorType.Intel_Atom:
                    return By.CssSelector(string.Format(processorLocator, ProcessorType.Intel_Atom.ToString().ToLower().Replace("_", "")));
                case ProcessorType.Samsung:
                    return By.CssSelector(string.Format(processorLocator, ProcessorType.Samsung.ToString().ToLower().Replace("_", "")));
                case ProcessorType.Tegra_K1:
                    return By.CssSelector(string.Format(processorLocator, ProcessorType.Tegra_K1.ToString().ToLower().Replace("_", "")));
                case ProcessorType.Intel_Core_i7:
                    return By.CssSelector(string.Format(processorLocator, ProcessorType.Intel_Core_i7.ToString().ToLower().Replace("_", "")));
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
                _driver.Click(GetLocatorForProcessorType(item));
            }

        }
    }

}
