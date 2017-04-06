using OpenQA.Selenium;

namespace OnlinerTests.Pages
{
    public class LoginComponent
    {
        private WebDriver _driver;

        public LoginComponent(WebDriver driver)
        {
            _driver = driver;
        }

        #region locators
        public By OpenLoginButtonLocator { get; set; } = By.XPath("//*[contains(@class,'auth-bar__item--text')]");
        public By LoginButtonLocator { get; set; } = By.XPath("//button[contains(@class,'auth-box__auth-submit')]");
        public By AccountTitleLocator { get; set; } = By.XPath("//a[contains(@data-bind,'nickname')]");
        #endregion

        public enum Credentials
        {
            Login,
            Password
        }

        public By GetInputPriceLocator(Credentials credentials)
        {
            string filterLocator = "//input[contains(@data-bind,'login.data.{0}')]";
            switch (credentials)
            {
                case Credentials.Login:
                    return By.XPath(string.Format(filterLocator, "login"));
                case Credentials.Password:
                    return By.XPath(string.Format(filterLocator, "password"));
                default:
                    throw new System.Exception($"Unknown order state: {credentials}");
            }
        }

        public void Open()
        {
            _driver.Navigate("https://onliner.by/#login");
        }

        public void Login(User user)
        {
            _driver.Click(OpenLoginButtonLocator);
            _driver.SendKeys(GetInputPriceLocator(Credentials.Login), user.Mailbox);
            _driver.SendKeys(GetInputPriceLocator(Credentials.Password), user.Password);
            _driver.Click(LoginButtonLocator);
        }
    }
}
