using NUnit.Framework;
using OnlinerTests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterProcessorTests : OnlinerTestsSetup
    {
        [TestCase("AMD A6")]
        public void FilterAMD_A6Test(string processorModel)
        {
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            notebooksPage.SelectProcessor(NotebooksPageOnliner.ProcessorType.AMD_A6);
            string[] prices = notebooksPage.GetDescription();
            foreach (var item in prices)
            {
                if (!item.Contains(processorModel))
                {
                    Assert.Fail("item with description " + item + " not contains model " + processorModel);
                }
            }
        }
    }
}
