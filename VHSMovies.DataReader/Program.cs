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

            bool updatePeople = args[1].Contains("up");

            bool registerTitles = args[1].Contains("rt");

            bool registerPeople = args[1].Contains("rp");

            string reviewerName = args.First();

            if (registerTitles)
            {
                ServiceLocator.GetInstance<SyncDataService>().RegisterNewData(reviewerName);
            }

            if (updateTitles)
            {
                ServiceLocator.GetInstance<SyncDataService>().UpdateTitles();
            }
        }
    }
}
