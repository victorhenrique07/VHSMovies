using MudBlazor.Services;

using Refit;

using VHSMovies.Api.Integration.Main.Clients;
using VHSMovies.Web;
using VHSMovies.Web.Components;
using VHSMovies.Web.Layout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var API_URL = builder.Environment.IsProduction() ? "http://localhost:5000" : "https://api.vhsmovies.com.br";

Action<HttpClient> httpClientConfigurator = c =>
{
    c.BaseAddress = new Uri(API_URL);
};

builder.Services.AddRefitClient<ITitlesClient>()
    .ConfigureHttpClient(httpClientConfigurator);
builder.Services.AddRefitClient<IGenresClient>()
    .ConfigureHttpClient(httpClientConfigurator);

builder.Services.AddSingleton<WebsiteDetails>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
