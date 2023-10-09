using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    /*IBookRepository de IRepositoryBase daki işlemleri tekrardan tanımladıktan sonra
    BookRepositoryde daha esnek şekilde kullanabiliyoruz örneğin Id ye veya isme göre sıralama vb. */
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters ,bool trackChanges)
        {
            var books = await FindAll(trackChanges)
                .FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice)
                .Search(bookParameters.SearchTerm)
                .Sort(bookParameters.OrderBy)
                .ToListAsync();

            return PagedList<Book>.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
        {
            //include işlemi ile kitapların relation içinde olduğu kategorileri de listeye ekledik
            return await _context.Books.Include(b => b.Category).OrderBy(b => b.Id).ToListAsync();
        }

        //API V2 için
        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(b => b.Id).ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id, bool trackChanges) =>
            await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
