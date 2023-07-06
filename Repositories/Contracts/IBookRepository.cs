using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    //IRepositoryBase de CRUD tanımlamış olmamıza rağmen burada tanımladık
   // sebebi ise BookRepositoryde daha esnek işlemeler yapabilmek için
   //kısaca işlemleri sınıflara göre özelleştirmiş olduk
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);
        Task<Book> GetBookByIdAsync(int id, bool trackChanges);
        void CreateOneBook(Book book);
        void UpdateOneBook(Book book);
        void DeleteOneBook(Book book);
    }
}
