using NUnit.Framework;
using OnlinerTests.Pages;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterProcessorsTests : TestsSetup
    {
        [TestCaseSource(typeof(ProcessorType), "processors")]
        public void FilterProcessorTest(string processorModel, FilterNotebooksCompoent.ProcessorType type)
        {
            var notebooksPage = new FilterNotebooksCompoent(_webDriver);
            notebooksPage.Open("https://catalog.onliner.by/notebook");
            notebooksPage.SelectProcessor(type);
            notebooksPage.WaitPageLoad();
            string[] prices = notebooksPage.GetDescription();
            foreach (var item in prices)
            {
                if (!item.Contains(processorModel))
                {
                    Assert.Fail("item with description " + item + " not contains model " + processorModel);
                }
            }
            string message = "filter processor: " + processorModel + "success";
            log.Pass(message);
        }
    }

    class ProcessorType
    {
        static object[] processors = {
        new object[] { "AMD A10", FilterNotebooksCompoent.ProcessorType.AMD_A10},
        new object[] { "AMD A6", FilterNotebooksCompoent.ProcessorType.AMD_A6},
        new object[] { "Intel Atom", FilterNotebooksCompoent.ProcessorType.Intel_Atom},
        new object[] { "Samsung", FilterNotebooksCompoent.ProcessorType.Samsung},
        new object[] { "Intel Core i7", FilterNotebooksCompoent.ProcessorType.Intel_Core_i7},
    };
    }
}
