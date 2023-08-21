using AspNetCoreRateLimit;
using EFCoreSample.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFCore;
using Services;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

//NLog konfigürasyonu ekleme
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers(config =>
{
    //api nin içerik pazarlýðýný açar
    config.RespectBrowserAcceptHeader = true;
    //api nin kabul etmediði içerikte bir yapý gelirse xml-csv gibi kabul eder
    config.ReturnHttpNotAcceptable = true;
    //caching profile ekler
    config.CacheProfiles.Add("5mins", new CacheProfile() { Duration = 300 });
})
    //içerik pazarlýðýnda xml kullanýmýna izin verir
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
    //.AddNewtonsoftJson();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

//ServicesExtensions içeriðini kullanmak için
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigurationLoggerManager();
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaTypes();
builder.Services.ConfigureBookLinks();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
//Rate Limit için
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
//auth için
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();


var app = builder.Build();

//ExceptionMiddlewareExceptions içindeki kullandýðýmýz servisleri GetRequiredService methodu ile ekledik
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

//Rate limit için
app.UseIpRateLimiting();

app.UseCors("CorsPolicy");

//Caching için kullandýk (microsoft caching in cors tan sonra çaðrýlmasýný öneriyor)
app.UseResponseCaching();

app.UseHttpCacheHeaders();

//auth için
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
