using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using VHSMovies.Api.Settings;
using VHSMovies.Application;
using VHSMovies.Infraestructure;
using VHSMovies.Mediator.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSimpleMediator();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 2L * 1024 * 1024 * 1024;
});

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy(Configuration.CorsPolicyName, policy =>
    {
        policy.WithOrigins( 
                Configuration.FrontendUrl
            )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VHS Movies API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseRouting();

app.UseCors(Configuration.CorsPolicyName);

app.UseAuthorization();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VHS Movies API V1");
        c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    });

    app.ApplyMigrations();
}

app.MapControllers();

app.Run();