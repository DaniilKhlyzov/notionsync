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
            options.AddArgument("user-data-dir=C:\\angel\\AppData\\Local\\Google\\Chrome\\User Data\\Profile 1");
            IWebDriver driver = new ChromeDriver(options);
            LoginPage loginPage = new LoginPage();
            SharePage sharePage = new SharePage(driver);
            
            var emailPermissions = new Dictionary<string, string>();
            var url = "https://www.notion.so/c895cd4719b74ffcaad491901689a8e9";
            using (var reader = new StreamReader("C:\\Users\\angel\\OneDrive\\Документы\\GitHub\\notionsync\\ConsoleApp5\\ConsoleApp5\\test.txt"))
            {
                var line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');

                    if (parts.Length == 2)
                    {
                        var email = parts[0].Trim();
                        var permissions = parts[1].Trim();
                        emailPermissions[email] = permissions;
                    }
                    else
                    {
                        Console.WriteLine("Некорректный формат строки: " + line);
                    }
                }
            }
            var emailPermissionsOnPage = sharePage.Filter(sharePage.ReadMembersRightsOnPage(url));
            foreach (var emailPermissionOnPage in emailPermissionsOnPage)
            {
                if (emailPermissions.ContainsKey(emailPermissionOnPage.Key))
                {
                    var permissions = emailPermissions[emailPermissionOnPage.Key];
                    if (permissions != emailPermissionOnPage.Value)
                    {
                        sharePage.AddMember(emailPermissionOnPage.Key, url);
                        if (permissions == "Can edit")
                        {
                            sharePage.CanEdit(emailPermissionOnPage.Key, url);
                        }
                        if (permissions == "Cancomment")
                        {
                            sharePage.CanComment(emailPermissionOnPage.Key, url);
                        }
                        if (permissions == "Can view")
                        {
                            sharePage.CanView(emailPermissionOnPage.Key, url);
                        }
                    }
                    sharePage.Remove(emailPermissionOnPage.Key, url);
                    emailPermissions.Remove(emailPermissionOnPage.Key);
                }

            }
            foreach (var emailPermission in emailPermissions)
            {
                var permissions = emailPermissions[emailPermission.Key];
                sharePage.AddMember(emailPermission.Key, url);
                if (permissions == "Can edit")
                {
                    sharePage.CanEdit(emailPermission.Key, url);
                }
                if (permissions == "Cancomment")
                {
                    sharePage.CanComment(emailPermission.Key, url);
                }
                if (permissions == "Can view")
                {
                    sharePage.CanView(emailPermission.Key, url);
                }
  
            }
            Console.WriteLine("Всё готово Хозяин");
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
            Thread.Sleep(100); 
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
        public List<string> ReadMembersRightsOnPage(string url)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            Driver.Navigate().GoToUrl(url);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            ClickShare();
            Thread.Sleep(5000);
            // JavaScript-скрипт для извлечения текста элементов с классом "notionslate" и их вложенных элементов
            string script = @"let texts = []; 
                  let elements = document.getElementsByClassName('notranslate');
                  for (var i = 0; i < elements.length; i++)
                  {
                      let element = elements[i];
                      let text = element.textContent || element.innerText;
                      texts.push(text);
                  }
                  return texts;";


            var listofguest = new List<string>();
            var extractedTexts = (ReadOnlyCollection<object>)jsExecutor.ExecuteScript(script);
            foreach (var text in extractedTexts)
            {
                var nnew = text.ToString();
                if (!String.IsNullOrEmpty(nnew))
                {
                    listofguest.Add(text.ToString());
                }
            }
            foreach (var text in listofguest)
                Console.WriteLine(text);
            return listofguest;
            
        }
        public Dictionary<string,string> Filter(List<string> mas)
        {
            var slovar = new Dictionary<string, string>();
            for (int i = 0; i < mas.Count; i++)
            {
                Console.WriteLine(mas[i]);
                var str = mas[i];
                if (str.Contains("Guest") && str.Length > 10)
                {
                    var num = str.IndexOf("Guest");
                    str = str.Substring(num + 5);
                    if (str.Contains("Can edit"))
                    {
                        slovar.Add(str.Substring(0, str.IndexOf("Can edit")), str.Substring(str.IndexOf("Can edit")));
                    }
                    if (str.Contains("Can view"))
                    {
                        slovar.Add(str.Substring(0, str.IndexOf("Can view")), str.Substring(str.IndexOf("Can view")));
                    }
                    if (str.Contains("Can comment"))
                    {
                        slovar.Add(str.Substring(0, str.IndexOf("Can comment")), str.Substring(str.IndexOf("Can comment")) );
                    }
                    if (str.Contains("Full access"))
                    {
                        slovar.Add(str.Substring(0, str.IndexOf("Full access")), str.Substring(str.IndexOf("Full access")) );
                    }
                }
            }
        return slovar;
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
            Thread.Sleep(1000);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(100));
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
