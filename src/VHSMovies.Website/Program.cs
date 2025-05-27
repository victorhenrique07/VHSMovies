using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor.Services;

using Refit;

using VHSMovies.Api.Integration.Main.Clients;
using VHSMovies.Website;
using VHSMovies.Website.Layout;
using VHSMovies.Website.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var API_URL = builder.HostEnvironment.IsDevelopment() ? "http://localhost:5000" : "https://api.vhsmovies.com.br";

Action<HttpClient> httpClientConfigurator = c =>
{
    c.BaseAddress = new Uri(API_URL);
};

builder.Services.AddRefitClient<ITitlesClient>()
    .ConfigureHttpClient(httpClientConfigurator);
builder.Services.AddRefitClient<IGenresClient>()
    .ConfigureHttpClient(httpClientConfigurator);

builder.Services.AddSingleton<WebsiteDetails>();

await builder.Build().RunAsync();
