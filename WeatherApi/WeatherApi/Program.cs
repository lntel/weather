using FluentValidation;
using WeatherApi.Config;
using WeatherApi.Models;
using WeatherApi.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiConfig>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddScoped<IValidator<GeoPoint>, GeoPointValidator>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
