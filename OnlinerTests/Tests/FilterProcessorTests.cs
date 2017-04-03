using NUnit.Framework;
using OnlinerTests.Pages;

namespace OnlinerTests
{
    [TestFixture]
    [Parallelizable]
    public class FilterProcessorTests : OnlinerTestsSetup
    {
        [TestCaseSource(typeof(ProcessorType), "processors")]
        public void FilterProcessorTest(string processorModel, NotebooksPageOnliner.ProcessorType type)
        {
            var notebooksPage = new NotebooksPageOnliner(_webDriver);
            notebooksPage.Open();
            notebooksPage.SelectProcessor(type);
            notebooksPage.WaitAjaxResponse();
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
        new object[] { "AMD A10", NotebooksPageOnliner.ProcessorType.AMD_A10},
        new object[] { "AMD A6", NotebooksPageOnliner.ProcessorType.AMD_A6}
    };
    }
}
