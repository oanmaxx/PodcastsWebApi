using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var elemText = element.GetAttribute("text");
            Assert.IsTrue("Login".Equals(elemText) || "Logout".Equals(elemText));
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
