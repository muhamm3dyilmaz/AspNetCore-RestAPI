using Entities.Models;
using Entities.RequestFeatures;
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
        Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
        Task<Book> GetBookByIdAsync(int id, bool trackChanges);
        void CreateOneBook(Book book);
        void UpdateOneBook(Book book);
        void DeleteOneBook(Book book);

        //book details için
        Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges);

        ////////////// API V2 İÇİN EKLEDİKLERİMİZ ////////////////
        Task<List<Book>> GetAllBooksAsync(bool trackChanges);

    }
}
