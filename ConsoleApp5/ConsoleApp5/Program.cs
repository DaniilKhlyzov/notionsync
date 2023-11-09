using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using SeleniumExtras.WaitHelpers;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:\\AcerGrey\\AppData\\Local\\Google\\Chrome\\User Data\\Profile2");
            IWebDriver driver = new ChromeDriver(options);

            try
            {
                LoginPage loginPage = new LoginPage();
                SharePage sharePage = new SharePage();

                //loginPage.Login(driver,"hifizov@yandex.ru", "3730133vv");
                sharePage.AddMember(driver, "dankhlyzov@gmail.com",
                    "https://www.notion.so/6890e249352f423f8d560cfcc863e844");
                Thread.Sleep(10000);
                sharePage.Remove(driver, "dankhlyzov@gmail.com",
                    "https://www.notion.so/6890e249352f423f8d560cfcc863e844");
            }
            finally
            {
                // Закрытие веб-драйвера и освобождение ресурсов
                driver.Quit();
                driver.Dispose();
            }
        }
    }

    public class LoginPage
    {
    
        public void Login(IWebDriver driver, string login, string password)
        {
            driver.Navigate().GoToUrl("https://www.notion.so/login");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            var emailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("notion-email-input-2")));
            emailInput.SendKeys(login);
            emailInput.SendKeys(Keys.Enter);
            var code = Console.ReadLine();
            var passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("notion-password-input-1")));
            passwordInput.SendKeys(password);
            passwordInput.SendKeys(Keys.Enter);
        }
    }

    public class SharePage
    {
        private WebDriverWait wait;

        public void AddMember(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl("https://www.notion.so");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            ClickShare(driver);
            InputMember(driver,member);
        }

        public void Remove(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            ClickShare(driver);
            ClickMember(member);
            var remove = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Remove')]")));
            remove.Click();
        }
        public void CanEdit(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            ClickShare(driver);
            ClickMember(member);
            var canEdit = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can edit')]")));
            canEdit.Click();
        }
        public void CanView(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            ClickShare(driver);
            ClickMember(member);
            var canView = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can view')]")));
            canView.Click();
        }
        public void CanComment(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            ClickShare(driver);
            ClickMember(member);
            var canComment = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'R')]")));
            canComment.Click();
        }

        private void ClickShare(IWebDriver driver)
        {
            var share = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Share')]")));
            share.Click();
        }

        private void InputMember(IWebDriver driver,string member)
        {
            var addMember = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#notion-app > div > div.notion-overlay-container.notion-default-overlay-container > div:nth-child(2) > div > div:nth-child(2) > div:nth-child(2) > div > div > div > div > div > div > div:nth-child(2) > div:nth-child(1) > div > div:nth-child(1) > div.notion-scroller.vertical.horizontal > div > input[type=email]")));
            addMember.SendKeys(member);
            var invite = driver.FindElement(By.XPath("//div[contains(text(), 'Invite')]"));
            invite.SendKeys(Keys.Enter);
            invite.Click();
        }

        private void ClickMember(string member)
        {
            var memberr = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'" + member + "' )]")));
            memberr.Click();
        }
        // to do общий метод для remove, can edit, can comment, can view
    }

}