using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.DevTools.V129.Network;
using VHSMovies.DataReader;
using VHSMovies.Domain.Infraestructure;
using VHSMovies.Domain.Infraestructure.Services;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Uso: dotnet run <source> <operation>");
            Console.WriteLine("Exemplo: dotnet run imdb rt");
            return;
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        using (WebDriverManager manager = new WebDriverManager())
        {
            ServiceLocator.Configure(configuration, manager);

            string reviewerName = args[0];
            string operation = args[1];

            bool updateReviews = operation.Contains("ur", StringComparison.OrdinalIgnoreCase);
            bool updateTitlesGenres = operation.Contains("utg", StringComparison.OrdinalIgnoreCase);

            if (!updateReviews && !updateTitlesGenres)
            {
                Console.WriteLine($"Operação inválida: {operation}");
                Console.WriteLine("Operações válidas: ur (Atualizar Reviews) & utg (Atualizar gêneros dos títulos).");
                return;
            }

            using (var scope = ServiceLocator.GetInstance<IServiceScopeFactory>().CreateScope())
            {
                var syncDataService = scope.ServiceProvider.GetRequiredService<SyncDataService>();

                try
                {
                    if (updateReviews)
                    {
                        Console.WriteLine($"Iniciando atualização de dados de: {reviewerName}");
                        await syncDataService.UpdateTitlesAsync(reviewerName);
                        Console.WriteLine("Atualização concluída com sucesso.");
                    }

                    if (updateTitlesGenres)
                    {
                        Console.WriteLine($"Iniciando atualização de gêneros de títulos em: {reviewerName}");
                        await syncDataService.UpdateTitlesGenres(reviewerName);
                        Console.WriteLine("Atualização concluída com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro durante a execução: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            }

            ServiceLocator.Dispose();
        }
    }
}
