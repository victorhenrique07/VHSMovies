using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.DevTools.V129.Network;
using VHSMovies.DataReader;
using VHSMovies.Domain.Infraestructure;
using VHSMovies.Domain.Infraestructure.Services;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        using (WebDriverManager manager = new WebDriverManager())
        {
            ServiceLocator.Configure(configuration, manager);

            bool updateTitles = args[1].Contains("ut");

            bool registerTitles = args[1].Contains("rt");

            string reviewerName = args.First();

            if (registerTitles)
            {
                ServiceLocator.GetInstance<SyncDataService>().RegisterNewData(reviewerName);
            }

            if (updateTitles)
            {
                ServiceLocator.GetInstance<SyncDataService>().UpdateTitles(reviewerName);
            }

            ServiceLocator.Dispose();
        }
    }
}
