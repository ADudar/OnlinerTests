using OpenQA.Selenium;
using System;

namespace OnlinerCore.Pages
{
    public class FilterRefrigeratorsComponent : BaseFilterComponent
    {

        public enum EnergyClass
        {
            APlusPlusPlus,
            APlusPlus,
            APlus,
            A,
            B,
            C,
            D,
        }

        public FilterRefrigeratorsComponent(WebDriver driver) : base(driver) { }

        public By GetLocatorEnergyClass(EnergyClass energyClass)
        {
            var energyClassLocator = "input[value={0}]+span";
            switch (energyClass)
            {
                case EnergyClass.APlusPlusPlus:
                    return By.CssSelector(string.Format(energyClassLocator, "a4"));
                case EnergyClass.APlusPlus:
                    return By.CssSelector(string.Format(energyClassLocator, "a2plus"));
                case EnergyClass.APlus:
                    return By.CssSelector(string.Format(energyClassLocator, "aplus"));
                case EnergyClass.A:
                    return By.CssSelector(string.Format(energyClassLocator, "a"));
                case EnergyClass.B:
                    return By.CssSelector(string.Format(energyClassLocator, "b"));
                case EnergyClass.C:
                    return By.CssSelector(string.Format(energyClassLocator, "c"));
                case EnergyClass.D:
                    return By.CssSelector(string.Format(energyClassLocator, "d"));
                default:
                    throw new Exception($"Unknown order state: {energyClass}");
            }
        }

        public void SelectEnergyClass(EnergyClass[] energyClass)
        {
            foreach (var item in energyClass)
            {
                if (!EnergyClassIsChecked(item))
                {
                    _driver.Scroll((_driver.WaitElementClickable(GetLocatorEnergyClass(item)).Location.Y));
                    _driver.Click(GetLocatorEnergyClass(item));
                }
            }
        }

        private bool EnergyClassIsChecked(EnergyClass classType)
        {
            var locator = GetLocatorEnergyClass(classType).ToString();
            locator = locator.Substring(locator.IndexOf(' '), locator.Length - locator.IndexOf(' '));
            string script = $"return window.getComputedStyle(document.querySelector('{locator}'),':before').getPropertyValue('opacity')";
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver.Driver;
            string opacity = (string)js.ExecuteScript(script);
            if (opacity == "1") return true;
            return false;
        }
    }
}
