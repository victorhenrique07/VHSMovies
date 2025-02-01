using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;
using VHSMovies.Api.Integration.Main.Clients;
using VHSMovies.Website;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

Action<HttpClient> httpClientConfigurator = c =>
        c.BaseAddress = new Uri("http://localhost:5000");

builder.Services.AddRefitClient<ITitlesClient>()
    .ConfigureHttpClient(httpClientConfigurator);


await builder.Build().RunAsync();
