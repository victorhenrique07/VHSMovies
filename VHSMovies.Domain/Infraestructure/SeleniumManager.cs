using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Infraestructure
{
    public class SeleniumManager : IHtmlReader
    {
        private readonly WebDriverManager webDriverManager;

        public SeleniumManager(WebDriverManager webDriverManager)
        {
            this.webDriverManager = webDriverManager;
        }

        public HtmlDocument Read(string url)
        {
            IWebDriver webDriver = webDriverManager.WebDriver;

            webDriver.Navigate().GoToUrl(url);

            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(webDriver.PageSource);

            return document;
        }
    }
}
