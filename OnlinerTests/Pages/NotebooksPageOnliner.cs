using OpenQA.Selenium;

namespace OnlinerTests.Pages
{
    public class NotebooksPageOnliner : CatalogPageOnliner
    {
        public enum ProcessorType
        {
            AMD_A10,
            AMD_A6,
            Intel_Atom,
            Samsung,
            Tegra_K1,
            Intel_Core_i7,
            Intel_Core_i5,
            Intel_Core_i3,
        }

        public NotebooksPageOnliner(WebDriver driver) : base(driver) { }

        #region locators
        public By ProcessorSelectLocator { get; set; } = By.XPath(".//*[@id='schema-filter']/div[1]/div[12]/div[3]/div[1]/div/span[2]");
        public By AMD_A10Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=amda10]+span");
        public By AMD_A6Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=amda6]+span");
        public By Intel_AtomLocator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=intelatom]+span");
        public By SamsungLocator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=samsung]+span");
        public By Tegra_K1Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=tegrak1]+span");
        public By Intel_Core_i7Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=intelcorei7]+span");
        public By Intel_Core_i5Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=intelcorei5]+span");
        public By Intel_Core_i3Locator { get; set; } = By.CssSelector(".schema-filter-popover_visible input[value=intelcorei3]+span");

        public By FrequencyMinLocator { get; set; } = By.XPath(".//*[@id='schema-filter']/div[1]/div[13]/div[3]/div/div[1]/input");
            #endregion

        public void Open()
        {
            _driver.Navigate("https://catalog.onliner.by/notebook");
        }

        public void SelectProcessor(ProcessorType t)
        {
            var selectButton = _driver.GetElementWithWaiting(ProcessorSelectLocator);

            _driver.Scroll(selectButton.Location.Y);
            _driver.Click(ProcessorSelectLocator);
            switch (t)
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
                case ProcessorType.Tegra_K1:
                    _driver.Click(Tegra_K1Locator);
                    break;
                case ProcessorType.Intel_Core_i7:
                    _driver.Click(Intel_Core_i7Locator);
                    break;
                case ProcessorType.Intel_Core_i5:
                    _driver.Click(Intel_Core_i5Locator);
                    break;
                case ProcessorType.Intel_Core_i3:
                    _driver.Click(Intel_Core_i3Locator);
                    break;
                default:
                    break;
            }
        }
    }
}
