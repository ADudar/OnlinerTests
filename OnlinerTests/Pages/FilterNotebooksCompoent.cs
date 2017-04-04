using OpenQA.Selenium;
using System;

namespace OnlinerTests.Pages
{
    public class FilterNotebooksCompoent : ResultsComponent
    {
        [Flags]
        public enum ProcessorType
        {
            None = 0,
            AMD_A10 = 1,
            AMD_A6 = 2,
            Intel_Atom = 4,
            Samsung = 8,
            Tegra_K1 = 16,
            Intel_Core_i7 = 32,
            Intel_Core_i5 = 64,
            Intel_Core_i3 = 128
        }

        public FilterNotebooksCompoent(WebDriver driver) : base(driver) { }

        #region locators
        public By ProcessorSelectLocator { get; set; } = By.XPath("//span[contains(text(),'Процессор')]/ancestor::*/following-sibling::div/child::div[contains(@data-bind,'click')]");
        public By AMD_A10Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=amda10]+span");
        public By AMD_A6Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=amda6]+span");
        public By Intel_AtomLocator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=intelatom]+span");
        public By SamsungLocator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=samsung]+span");
        public By Intel_Core_i7Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=intelcorei7]+span");
        #endregion

        //public void SelectProcessor(params ProcessorType[] processorTypes)
        public void SelectProcessor(ProcessorType processorTypes)
        {
            //SelectProcessor(ProcessorType.Intel_Atom, ProcessorType.AMD_A10, ProcessorType.Samsung);

            var selectButton = _driver.WaitElementClickable(ProcessorSelectLocator);

            _driver.Scroll(selectButton.Location.Y);
            _driver.Click(ProcessorSelectLocator);
            switch (processorTypes)
            {
                case ProcessorType.AMD_A10:
                    _driver.Click(AMD_A10Locator);
                    break;
                case ProcessorType.AMD_A6:
                    _driver.Click(AMD_A6Locator);
                    break;
                case ProcessorType.Intel_Atom:
                    _driver.Click(Intel_AtomLocator);
                    break;
                case ProcessorType.Samsung:
                    _driver.Click(SamsungLocator);
                    break;
                case ProcessorType.Intel_Core_i7:
                    _driver.Click(Intel_Core_i7Locator);
                    break;
                default:
                    break;
            }
        }
    }
}
