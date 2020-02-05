using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace GK.Automation.Web
{
    [TestFixture]
    class SeleniumIntegrationTests
    {
        IWebDriver _driver;
        string homeUrl = "https://supersport.com/search";

        [SetUp]
        public void TestFixtureSetUp()
        {
            _driver = new ChromeDriver();
        }

        [TearDown]
        public void TestFixtureTearDown()
        {
            _driver.Close();
        }

        [Test]
        public void TestSearchFieldEmptyOnFirstLoad()
        {
            _driver.Navigate().GoToUrl(homeUrl);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            var searchFieldElement = WaitForAndGetElement(wait, By.Id("srch-term"));
            Assert.AreEqual("", searchFieldElement.Text);
        }

        [Test]
        public void TestSearchTermPassedThrough()
        {
            _driver.Navigate().GoToUrl(homeUrl);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            var searchFieldElement = WaitForAndGetElement(wait, By.Id("srch-term"));
            searchFieldElement.SendKeys("Cricket");

            var searchFieldButton = _driver.FindElement(By.ClassName("input-group-btn"));
            searchFieldButton.Click();

            var newsLabelElement = WaitForAndGetElement(wait, By.Id("srch-term"));
            Assert.AreEqual("Cricket", newsLabelElement.GetAttribute("value"));
        }

        [Test]
        public void TestSearchTermBringsUpResults()
        {
            _driver.Navigate().GoToUrl(homeUrl);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            var searchFieldElement = WaitForAndGetElement(wait, By.Id("srch-term"));
            searchFieldElement.SendKeys("Cricket");

            var searchFieldButton = _driver.FindElement(By.ClassName("input-group-btn"));
            searchFieldButton.Click();

            var newsLabelElement = WaitForAndGetElement(wait, By.XPath("//*[@id=\"news-listing\"]/div[1]/div/span"));
            Assert.AreEqual("News", newsLabelElement.Text);
        }

        private IWebElement WaitForAndGetElement(WebDriverWait wait, By by)
        {
            IWebElement searchElement;
            wait.Until(x => x.FindElement(by));
            searchElement = _driver.FindElement(by);
            return searchElement;
        }
    }
}
