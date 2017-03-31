
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace OnlinerTests.Pages
{
    public class NotebooksPageOnliner : CatalogPageOnliner
    {

        public By AMD_A6Locator { get; set; } = By.XPath("//*[@id='schema-filter']/div[1]/div[11]/div[3]/div[2]/div/div/div/div[18]/label/span[2]");
        public By ProcessorSelectLocator { get; set; } = By.XPath("//*[@id='schema-filter']/div[1]/div[11]/div[3]/div[1]/div/span[2]");
        //public By LenovoLocator { get; set; } = By.XPath(".//*[@id='schema-filter']/div[1]/div[2]/div[2]/ul/li[1]/label/span[1]/span");

        public NotebooksPageOnliner(WebDriver driver) : base(driver) { }

        public void Open()
        {
            _driver.Navigate("https://catalog.onliner.by/notebook");
        }

        public enum ProcessorType
        {
            AMD_A6
        }


        public void SelectProcessor(ProcessorType t)
        {
            _driver.WaitPageLoaded();
            _driver.WaitWhileElementClassContainsText(LoadingProductLocator, "schema-products_processing");
            _driver.WaitWhileElementClassContainsText(SchemaFilterButtonLocator, "schema-filter-button__state_animated");
            _driver.Click(ProcessorSelectLocator);
            switch (t)
            {
                case ProcessorType.AMD_A6:
                    _driver.Click(AMD_A6Locator);
                    break;
                default:
                    break;
            }
        }
    }
}
