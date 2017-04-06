using NUnit.Framework;
using OnlinerTests.Pages;
using System;
using System.Linq;

namespace OnlinerTests
{
    using static FilterRefrigeratorsComponent;
    using static FilterRefrigeratorsComponent.EnergyClass;

    [TestFixture]
    [Parallelizable]
    public class FilterEnergyClassTests : TestsSetup
    {
        [TestCaseSource(typeof(EnergiesData), "energies")]
        public void FilterEnergyClassTest(EnergyClass[] energyClass)
        {
            var refrigeratorsComponent = new FilterRefrigeratorsComponent(_webDriver);
            var resultsComponent = new ResultsComponent(_webDriver);
            resultsComponent.Open("https://catalog.onliner.by/refrigerator");
            refrigeratorsComponent.SelectEnergyClass(energyClass);
            resultsComponent.WaitPageLoad();
            string[] description = resultsComponent.GetDescription();
            if (!description.All(descr => energyClass.Any(energy => descr.Contains(energy.ToString().Replace("Plus", "+")))))
            {
                Assert.Fail();
            }
            string message = "filter refigerators success";
            log.Pass(message);
        }
    }

    class EnergiesData
    {
        static object[] energies = {
        new EnergyClass[] {  APlusPlusPlus, APlusPlus },
        //new EnergyClass[] {  A, APlusPlus }
    };
    }
}
