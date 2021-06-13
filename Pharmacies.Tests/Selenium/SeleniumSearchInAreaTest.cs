// Generated by Selenium IDE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Xunit;
using Pharmacies.Tests;


namespace Pharmacies.Tests
{
    public class SeleniumSearchInAreaTest : SeleniumBase
    {

        [Fact]
        public void SearchInArea()
        {
            driver.Navigate().GoToUrl("https://localhost:44348/");
            driver.Manage().Window.Size = new System.Drawing.Size(1936, 1066);
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".blazored-typeahead__input")).Click(); 
            driver.FindElement(By.CssSelector(".blazored-typeahead__input")).SendKeys("Hał");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".blazored-typeahead__result")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".blazored-typeahead__clear")).Click();
        }
    }
}