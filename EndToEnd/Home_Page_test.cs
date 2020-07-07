using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EndToEnd
{
    [TestFixture]   //atribut pt o clasa de test
    public class Home_Page_test
    {
        private IWebDriver driver;
        public string homeURL;

        //metode de test -> are atribut Test 
        [Test(Description = "Check PodcastsWebApi Homepage for Login")]
        public void Login_is_on_home_page()
        {
            driver.Navigate().GoToUrl(this.homeURL);
            driver.Manage().Window.Maximize();
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(600));
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button")));
            IWebElement element = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button"));

            var elemText = element.Text;
            Assert.IsTrue("Login".Equals(elemText) || "Logout".Equals(elemText));
        }

        [Test(Description = "Check PodcastsWebApi Homepage for Login and Logout.")]
        public void Login_Logout_User_on_home_page()
        {
            driver.Navigate().GoToUrl(this.homeURL);
            driver.Manage().Window.Maximize();
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(600));
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button")));
            IWebElement loginLogoutBut = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button"));

            Assert.IsTrue("Login".Equals(loginLogoutBut.Text));

            // login
            IWebElement emailElement = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/mat-form-field[1]/div/div[1]/div/input"));
            IWebElement passElement = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/mat-form-field[2]/div/div[1]/div/input"));

            emailElement.SendKeys("oan.max@yahoo.com");
            passElement.SendKeys("Qwerty2020");
            loginLogoutBut.Click();

            // wait until podcasts button appears
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/body/app-nav-menu/header/nav/div/div/ul/li[2]/a")));
            IWebElement podcasts = driver.FindElement(By.XPath("/html/body/app-root/body/app-nav-menu/header/nav/div/div/ul/li[2]/a"));
            Assert.IsNotNull(podcasts);

            loginLogoutBut = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button"));
            Assert.IsTrue("Logout".Equals(loginLogoutBut.Text));

            driver.Navigate().Refresh();

            // check that login is kept
            loginLogoutBut = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button"));
            Assert.IsTrue("Logout".Equals(loginLogoutBut.Text));

            // logout
            loginLogoutBut.Click();

            // wait for confirmation window
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div/div[2]/div/mat-dialog-container/confirm-dialog/div[2]/button[1]")));

            // click the confoirmation button
            IWebElement confirmOk = driver.FindElement(By.XPath("/html/body/div/div[2]/div/mat-dialog-container/confirm-dialog/div[2]/button[1]"));
            confirmOk.Click();

            // check that login is again displayed
            driver.Navigate().Refresh();
            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button")));
            loginLogoutBut = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-home/div/button"));

            var text = loginLogoutBut.Text;
            Assert.IsTrue("Login".Equals(text));
        }

        //se apeleaza dupa fiecare test
        [TearDown]
        public void TearDownTest()
        {
            driver.Close();
        }

        //se executa inaintea oricarei metode de test -> initiliazeaza necesarul(field-uri, colectii) pt test
        [SetUp]
        public void SetupTest()
        {
            homeURL = "https://localhost:44358/";
            driver = new ChromeDriver();

        }
    }
}
