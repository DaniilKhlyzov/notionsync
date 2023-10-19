using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:\\AcerGrey\\AppData\\Local\\Google\\Chrome\\User Data\\Profile2");
            IWebDriver driver = new ChromeDriver(options);
            //login(driver, "hifizov@yandex.ru", "3730133vv");
            AddMember(driver, "dankhlyzov@gmail.com", "https://www.notion.so/6890e249352f423f8d560cfcc863e844");
            CanComment(driver, "dankhlyzov@gmail.com", "https://www.notion.so/6890e249352f423f8d560cfcc863e844");
            Thread.Sleep(10000);
            Remove(driver, "dankhlyzov@gmail.com", "https://www.notion.so/6890e249352f423f8d560cfcc863e844");

        }
        public void login(IWebDriver driver, string login, string password)
        {
            driver.Navigate().GoToUrl("https://www.notion.so/login");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("notion-email-input-2")));
            var emailInput = driver.FindElement(By.Id("notion-email-input-2"));
            emailInput.SendKeys(login);
            emailInput.SendKeys(OpenQA.Selenium.Keys.Enter);
            var code = Console.ReadLine();
            var elements = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("notion-password-input-1")));
            var passwordInput = driver.FindElement(By.Id("notion-password-input-1"));
            passwordInput.SendKeys(password);
            passwordInput.SendKeys(OpenQA.Selenium.Keys.Enter);
        }
        public void AddMember(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl("https://www.notion.so");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Share')]")));
            var share = driver.FindElement(By.XPath("//div[contains(text(), 'Share')]"));
            share.Click();
            var element2 = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#notion-app > div > div.notion-overlay-container.notion-default-overlay-container > div:nth-child(2) > div > div:nth-child(2) > div:nth-child(2) > div > div > div > div > div > div > div:nth-child(2) > div:nth-child(1) > div > div:nth-child(1) > div.notion-scroller.vertical.horizontal > div > input[type=email]")));
            var addMember = driver.FindElement(By.CssSelector("#notion-app > div > div.notion-overlay-container.notion-default-overlay-container > div:nth-child(2) > div > div:nth-child(2) > div:nth-child(2) > div > div > div > div > div > div > div:nth-child(2) > div:nth-child(1) > div > div:nth-child(1) > div.notion-scroller.vertical.horizontal > div > input[type=email]"));
            addMember.SendKeys(member);
            addMember.SendKeys(OpenQA.Selenium.Keys.Enter);

        }
        public void Remove(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
 
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Share')]")));
            var share = driver.FindElement(By.XPath("//div[contains(text(), 'Share')]"));
            share.Click();
            var element3 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]")));
            var memberr = driver.FindElement(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]"));
            memberr.Click();
            var element4 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Remove')]")));
            var remove = driver.FindElement(By.XPath("//div[contains(text(),'Remove')]"));
            remove.Click();

        }
        public void CanView(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Share')]")));
            var share = driver.FindElement(By.XPath("//div[contains(text(), 'Share')]"));
            share.Click();
            var element3 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]")));
            var memberr = driver.FindElement(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]"));
            memberr.Click();
            var element4 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can view')]")));
            var remove = driver.FindElement(By.XPath("//div[contains(text(),'Can view')]"));
            remove.Click();

        }
        public void CanComment(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Share')]")));
            var share = driver.FindElement(By.XPath("//div[contains(text(), 'Share')]"));
            share.Click();
            var element3 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]")));
            var memberr = driver.FindElement(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]"));
            memberr.Click();
            var element4 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can comment')]")));
            var remove = driver.FindElement(By.XPath("//div[contains(text(),'Can comment')]"));
            remove.Click();

        }
        public void CanEdit(IWebDriver driver, string member, string url)
        {
            driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Share')]")));
            var share = driver.FindElement(By.XPath("//div[contains(text(), 'Share')]"));
            share.Click();
            var element3 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]")));
            var memberr = driver.FindElement(By.XPath("//div[contains(text(),'dankhlyzov@gmail.com' )]"));
            memberr.Click();
            var element4 = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Can edit')]")));
            var remove = driver.FindElement(By.XPath("//div[contains(text(),'Can edit')]"));
            remove.Click();

        }
    }
}