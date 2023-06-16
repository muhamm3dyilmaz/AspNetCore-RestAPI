using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;
using Services.Contracts;
using Services;

namespace EFCoreSample.Extensions
{
    /*IServiceCollection interfacesini tanımını genişletip program.cs te yazdığımız bazı 
     serviceleri burada tanımlayıp program.cs te yazılan ifadeleri kısaltabiliriz*/
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("sqlConnection")
                    )
                );
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigurationLoggerManager(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerManager>();
        }

    }
}
