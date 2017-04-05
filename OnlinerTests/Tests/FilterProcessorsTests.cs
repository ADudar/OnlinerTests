using NUnit.Framework;
using OnlinerTests.Pages;
using System;
using System.Linq;

namespace OnlinerTests
{
    using static FilterNotebooksComponent;
    using static FilterNotebooksComponent.ProcessorType;

    [TestFixture]
    [Parallelizable]
    public class FilterProcessorsTests : TestsSetup
    {
        [TestCaseSource(typeof(ProcessorsData), "processors")]
        public void FilterProcessorTest(ProcessorType[] processors)
        {
            var notebooksComponent = new FilterNotebooksComponent(_webDriver);
            var resultsComponent = new ResultsComponent(_webDriver);
            resultsComponent.Open("https://catalog.onliner.by/notebook");
            notebooksComponent.SelectProcessor(processors);
            resultsComponent.WaitPageLoad();
            string[] description = resultsComponent.GetDescription();
            if (!description.All(descr => processors.Any(p => descr.Contains(p.ToString().Replace("_", " ")))))
            {
                Assert.Fail("item with description " + " not contains model ");
            }
            string message = "filter processor: success";
            log.Pass(message);
        }
    }

    class ProcessorsData
    {
        static object[] processors = {
        new ProcessorType[] {  AMD_A6, AMD_A10 },
        new ProcessorType[] {  Intel_Atom, Intel_Core_i7},
        new ProcessorType[] {  Intel_Atom, Intel_Core_i7}
    };
    }
}
