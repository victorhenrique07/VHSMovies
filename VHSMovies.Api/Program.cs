using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using VHSMovies.Api;
using VHSMovies.Application;
using VHSMovies.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 400 * 1024 * 1024;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
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

app.UseCors("AllowAll");

app.UseStaticFiles();

// Configure the HTTP request pipeline.
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();