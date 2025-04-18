using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;
using VHSMovies.Api.Integration.Main.Clients;
using VHSMovies.Website;
using VHSMovies.Website.Layout;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

Action<HttpClient> httpClientConfigurator = c =>
{
    c.BaseAddress = new Uri("https://up-monkfish-locally.ngrok-free.app");
    c.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
};

builder.Services.AddRefitClient<ITitlesClient>()
    .ConfigureHttpClient(httpClientConfigurator);
builder.Services.AddRefitClient<IGenresClient>()
    .ConfigureHttpClient(httpClientConfigurator);

builder.Services.AddSingleton<WebsiteDetails>();


await builder.Build().RunAsync();
