using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using OpenQA.Selenium.Support.Extensions;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp
{
    public static class Program
    {
        static void Main()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:\\AcerGrey\\AppData\\Local\\Google\\Chrome\\User Data\\Profile2");
            IWebDriver driver = new ChromeDriver(options);
            LoginPage loginPage = new();
            SharePage sharePage = new SharePage(driver);
            //cчитываем таблицу или файл, сравнение с тем что на странице, если есть все хорошо, следующий, если нет на странице добавлем, затем нужные права, если нет в списке удаляем, добавить возможность исключения, затем повторная проверка
            //loginPage.Login(driver, "dankhlyzov@gmail.com", "rWX-yFZ-PcN-AkA");
            //sharePage.AddMember( "dankhlyzov@gmail.com",
            // "https://www.notion.so/6890e249352f423f8d560cfcc863e844"); 
            //Thread.Sleep(10000);
            //sharePage.Remove(driver, "dankhlyzov@gmail.com",
            // "https://www.notion.so/6890e249352f423f8d560cfcc863e844");
            //sharePage.CanView( "dankhlyzov@gmail.com",
            //"https://www.notion.so/6890e249352f423f8d560cfcc863e844");
            var emailPermissions = new Dictionary<string, string>();
            var url = "https://www.notion.so/6890e249352f423f8d560cfcc863e844";
            sharePage.ReadMembersRightsOnPage(url);

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
        private WebDriver Driver { get; }
        public SharePage(IWebDriver driver)
        {
            Driver = (WebDriver)driver;
        }

        public string ReadMemberRightsOnPage(string url, string member)
        {
            Driver.Navigate().GoToUrl(url);
            ClickShare();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            //to do селектор сделать нормальным
            var addMember = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#notion-app > div > div.notion-overlay-container.notion-default-overlay-container > div:nth-child(2) > div > div:nth-child(2) > div:nth-child(2) > div > div > div > div > div > div > div:nth-child(2) > div:nth-child(1) > div > div:nth-child(1) > div.notion-scroller.vertical.horizontal > div > input[type=email]")));
            addMember.SendKeys(member);
            addMember.Click();
            Thread.Sleep(1000);
            var rights = Driver.FindElements(By.ClassName("notranslate"));
            return rights.Last().Text;
        }
        public void ReadMembersRightsOnPage(string url)
        {
            Driver.Navigate().GoToUrl(url);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            Thread.Sleep(100);
            // JavaScript-скрипт для извлечения текста элементов с классом "notionslate" и их вложенных элементов
            string script = "let elements = document.getElementsByClassName('notionslate');" +
                             "let texts = [];"+ 
                             "for (var i = 0; i < elements.length; i++)"+
                             "{"+
                                "let element = elements[i];"+
                                "let text = element.textContent || element.innerText;"+
                                "texts.push(text);"+
                              "}" +
                              "return texts;";
        


            var extractedTexts = (ReadOnlyCollection<object>)jsExecutor.ExecuteScript(script);
            List<string> extractedTextList = new List<string>();
            foreach (var text in extractedTexts)
            {
                extractedTextList.Add(text.ToString());
            }
            foreach (var extractedText in extractedTextList)
            {
                Console.WriteLine(extractedText);
            }
        }







        public void AddMember(string member, string url)
        {
            Driver.Navigate().GoToUrl(url);
            ClickShare();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            var addMember = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#notion-app > div > div.notion-overlay-container.notion-default-overlay-container > div:nth-child(2) > div > div:nth-child(2) > div:nth-child(2) > div > div > div > div > div > div > div:nth-child(2) > div:nth-child(1) > div > div:nth-child(1) > div.notion-scroller.vertical.horizontal > div > input[type=email]")));
            addMember.SendKeys(member);
            var invite = Driver.FindElement(By.XPath("//div[contains(text(), 'Invite')]"));
            invite.SendKeys(Keys.Enter);
            invite.Click();
        }
        

        public void Remove(string member, string url)
        {
            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            ClickShare();
            ClickMember(member);
            var remove = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Remove')]")));
            remove.Click();
            
        }
        public void CanEdit(string member, string url)
        {
            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            ClickShare();
            ClickMember(member);
            var canEdit = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can edit')]")));
            canEdit.Click();
        }
        public void CanView( string member, string url)
        {
            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            ClickShare();
            ClickMember(member);
            var canView = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can view')]")));
            canView.Click();
        }
        public void CanComment(string member, string url)
        {
            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            ClickShare();
            ClickMember(member);
            var canComment = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can comment')]")));
            canComment.Click();
        }

        private void ClickShare()
        {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(40));
            var share = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Share')]")));
            share.Click();
        }

        private void ClickMember(string member)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            var memberr = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'" + member + "' )]")));
            memberr.Click();
        }
        // to do общий метод для remove, can edit, can comment, can view
    }

}
