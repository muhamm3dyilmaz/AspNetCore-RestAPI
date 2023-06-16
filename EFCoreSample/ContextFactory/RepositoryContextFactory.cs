using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

namespace EFCoreSample.ContextFactory
{
    //Migrationların konumlandırmasını ayarlamak için ve DbContext Create edebilmek için
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            //configurationBuilder -> appsetting.json dosyasına erişim izni verir
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            //DbContextOptionsBuilder -> appsetting.json dosyasındaki 'sqlConnection' alanına
            //erişir ve Migration kaydını 'EFCoreSample' projesi içine yapar
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                prj => prj.MigrationsAssembly("EFCoreSample"));

            return new RepositoryContext(builder.Options);
        }
    }
}
