using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;


namespace Tests
{
    class Test_E2E
    {

        private IWebDriver _driver;
        [SetUp]
        public void SetupDriver()
        {
            _driver = new ChromeDriver("C:\\Users\\danie\\Downloads\\chromedriver_win32 (1)");
        }
        

       [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

        [Test]
        public void LoginFormExists()
        {
            _driver.Url = "http://localhost:3000";


            var button = _driver.FindElement(By.CssSelector("[href=\"/login\"]"));
           
              button.Click();
              try
               {
                 IWebElement name = _driver.FindElement(By.XPath("//ion-input[@name='email']"));
                 IWebElement password = _driver.FindElement(By.XPath("//ion-input[@name='password']"));
                 IWebElement submitButton = _driver.FindElement(By.XPath("//ion-button[@type='submit']"));
                 Assert.Pass();


                    }
                    catch (NoSuchElementException)
                    {
                        Assert.Fail("Login form not found");
                    }
                
            
            Assert.Fail("Login form not found");

        }


        [Test]
        public void Login()
        {
            _driver.Url = "http://localhost:3000";
            var loginForm = _driver.FindElement(By.Id("login"));
            loginForm.Click();
            var emailInput = _driver.FindElement(By.XPath("//ion-input[@name='email']"));
            var password = _driver.FindElement(By.XPath("//ion-input[@name='password']"));
            emailInput.SendKeys("dani@example.com");
            password.SendKeys("Test123*");
            var loginBtn = _driver.FindElement(By.CssSelector("[type=\"submit\"]"));
            loginBtn.Click();
            var title = _driver.FindElement(By.Id("title"));
            Assert.IsTrue(title.Text.Contains("Task"));

        }


        [Test]
        public void RegisterFormExists()
        {
            _driver.Url = "http://localhost:3000";


            var button = _driver.FindElement(By.CssSelector("[href=\"/register\"]"));

            button.Click();
            try
            {
                IWebElement name = _driver.FindElement(By.Id("email"));
                IWebElement password = _driver.FindElement(By.Id("password"));
                IWebElement submitButton = _driver.FindElement(By.Id("submit"));
                Assert.Pass();


            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Login form not found");
            }


            Assert.Fail("Login form not found");

        }


    


}
}
