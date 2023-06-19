using EFCoreSample.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories.EFCore;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

//NLog konfig�rasyonu ekleme
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ServicesExtensions i�eri�ini kullanmak i�in
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigurationLoggerManager();


var app = builder.Build();

//ExceptionMiddlewareExceptions i�indeki kulland���m�z servisleri GetRequiredService methodu ile ekledik
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
