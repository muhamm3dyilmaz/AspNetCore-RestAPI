using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    //DbContext'i IdentityDbContext ile değiştir (user authentication için)
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {

        }
        
        //DbSet ile modelden verilen tablo adına veri aktarılır.
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        //Tip Konfigürasyonunu Veritabanına Yansıtır
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //user auth için ekledik
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new BookConfig());
            //modelBuilder.ApplyConfiguration(new RoleConfig());

            //IEntityTypeConfiguration ifadesini kullanan tüm configleri implemente eder (tek tek yazmaya gerek yok)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
