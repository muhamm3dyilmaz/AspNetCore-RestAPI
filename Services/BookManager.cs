using Entities.Models;
using Services.Contracts;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;

        public BookManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Book CreateBook(Book book)
        {
            if(book is null)
                throw new ArgumentNullException(nameof(book));
            
            _manager.BookRepo.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteBook(int id, bool trackChanges)
        {
            var entity = _manager.BookRepo.GetBookById(id, trackChanges);

            if(entity is null)
                throw new Exception($"Book with id: {id} could not found.");

            _manager.BookRepo.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.BookRepo.GetAllBooks(trackChanges);
        }

        public Book GetBookById(int id, bool trackChanges)
        {
            return _manager.BookRepo.GetBookById(id,trackChanges);
        }

        public void UpdateBook(int id, bool trackChanges, Book book)
        {
            var entity = _manager.BookRepo.GetBookById(id, trackChanges);
            if (entity is null)
                throw new Exception($"Book with id: {id} could not found.");

            if (book is null)
                throw new ArgumentNullException(nameof(book));

            entity.Title = book.Title;
            entity.Price = book.Price;

            _manager.BookRepo.UpdateOneBook(entity);
            _manager.Save();
        }
    }
}
