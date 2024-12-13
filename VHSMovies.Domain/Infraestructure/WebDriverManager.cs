using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Infraestructure
{
    public class WebDriverManager : IDisposable
    {
        private IWebDriver driver;

        public IWebDriver WebDriver
        {
            get
            {
                if (driver == null)
                    driver = new ChromeDriver();

                return driver;
            }
        }

        public void Dispose()
        {
            if (driver != null)
                driver.Dispose();
        }
    }
}
