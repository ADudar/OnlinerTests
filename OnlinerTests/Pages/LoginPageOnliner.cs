using OpenQA.Selenium;

namespace OnlinerTests.Pages
{
    public class LoginPageOnliner
    {
        private WebDriver _driver;

        public LoginPageOnliner(WebDriver driver)
        {
            _driver = driver;
        }

        #region locators
        public By OpenLoginButtonLocator { get; set; } = By.XPath("//*[contains(@class,'auth-bar__item--text')]");
        public By LoginInputLocator { get; set; } = By.XPath("//input[contains(@data-bind,'login.data.login')]");
        public By PassInputtLocator { get; set; } = By.XPath("//input[contains(@data-bind,'login.data.password')]");
        public By LoginButtonLocator { get; set; } = By.XPath("//button[contains(@class,'auth-box__auth-submit')]");
        public By AccountTitleLocator { get; set; } = By.XPath("//a[contains(@data-bind,'nickname')]"); 
        #endregion

        public void Open()
        {
            _driver.Navigate("https://onliner.by/#login");
        }

        public void Login(User user)
        {
            _driver.Click(OpenLoginButtonLocator);
            _driver.SendKeys(LoginInputLocator, user.Mailbox);
            _driver.SendKeys(PassInputtLocator, user.Password);
            _driver.Click(LoginButtonLocator);
        }
    }
}
