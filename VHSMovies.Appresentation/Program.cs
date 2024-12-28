using MudBlazor.Services;
using Refit;
using VHSMovies.Api.Integration.Client;
using VHSMovies.Appresentation.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl:DefaultApi")) });

Action<HttpClient> httpClient = c =>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl:DefaultApi"));

var refitSettings = new RefitSettings
{
    CollectionFormat = CollectionFormat.Csv
};

builder.Services.AddRefitClient<ITitleClient>()
    .ConfigureHttpClient(httpClient);

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
