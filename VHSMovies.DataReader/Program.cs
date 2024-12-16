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

            bool registerTitles = operation.Contains("rt", StringComparison.OrdinalIgnoreCase);
            bool updateTitles = operation.Contains("ut", StringComparison.OrdinalIgnoreCase);

            if (!registerTitles && !updateTitles)
            {
                Console.WriteLine($"Operação inválida: {operation}");
                Console.WriteLine("Operações válidas: rt (registrar títulos), ut (atualizar títulos)");
                return;
            }

            using (var scope = ServiceLocator.GetInstance<IServiceScopeFactory>().CreateScope())
            {
                var syncDataService = scope.ServiceProvider.GetRequiredService<SyncDataService>();

                try
                {
                    if (registerTitles)
                    {
                        Console.WriteLine($"Iniciando registro de novos dados para o revisor: {reviewerName}");
                        await syncDataService.RegisterNewData(reviewerName);
                        Console.WriteLine("Registro concluído com sucesso.");
                    }

                    if (updateTitles)
                    {
                        Console.WriteLine($"Iniciando atualização de dados para o revisor: {reviewerName}");
                        await syncDataService.UpdateTitlesAsync(reviewerName);
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
