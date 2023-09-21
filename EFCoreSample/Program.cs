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

//NLog konfig�rasyonu ekleme
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers(config =>
{
    //api nin i�erik pazarl���n� a�ar
    config.RespectBrowserAcceptHeader = true;
    //api nin kabul etmedi�i i�erikte bir yap� gelirse xml-csv gibi kabul eder
    config.ReturnHttpNotAcceptable = true;
    //caching profile ekler
    config.CacheProfiles.Add("5mins", new CacheProfile() { Duration = 300 });
})
    //i�erik pazarl���nda xml kullan�m�na izin verir
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
    //.AddNewtonsoftJson();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
//versiyonlamadan dolay� swagger error verdi ve yap�land�r�ld� -- ConfigureSwagger
//builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

//ServicesExtensions i�eri�ini kullanmak i�in
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
//swagger yap�land�rmas�
builder.Services.ConfigureSwagger();
//caching
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
//Rate Limit i�in
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
//auth i�in
builder.Services.ConfigureIdentity();
//jwt i�in
builder.Services.ConfigureJWT(builder.Configuration);


var app = builder.Build();

//ExceptionMiddlewareExceptions i�indeki kulland���m�z servisleri GetRequiredService methodu ile ekledik
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //versiyonlama kaynakl� swagger yap�land�rma
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore v1");
        s.SwaggerEndpoint("/swagger/v2/swagger.json", "BookStore v2");
    });
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

//Rate limit i�in
app.UseIpRateLimiting();

app.UseCors("CorsPolicy");

//Caching i�in kulland�k (microsoft caching in cors tan sonra �a�r�lmas�n� �neriyor)
app.UseResponseCaching();

app.UseHttpCacheHeaders();

//auth i�in
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
