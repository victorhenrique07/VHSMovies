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

builder.Services.AddHttpClient(
    Configuration.HttpClientName,
    client =>
    {
        client.BaseAddress = new Uri(Configuration.BackendUrl);
    });

builder.Services.AddRefitClient<ITitlesClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(Configuration.BackendUrl);
    });

builder.Services.AddRefitClient<IGenresClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(Configuration.BackendUrl);
    });

builder.Services.AddSingleton<WebsiteDetails>();


await builder.Build().RunAsync();
