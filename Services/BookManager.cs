using Entities.Models;
using Services.Contracts;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Exceptions;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;

        public BookManager(IRepositoryManager manager, ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public Book CreateBook(Book book)
        {   
            _manager.BookRepo.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteBook(int id, bool trackChanges)
        {
            var entity = _manager.BookRepo.GetBookById(id, trackChanges);

            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            _manager.BookRepo.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.BookRepo.GetAllBooks(trackChanges);
        }

        public Book GetBookById(int id, bool trackChanges)
        {
            var entity = _manager.BookRepo.GetBookById(id,trackChanges);

            //Entities katmanında yazdığımız exception kullanıyoruz
            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            return entity;
        }

        public void UpdateBook(int id, bool trackChanges, Book book)
        {
            var entity = _manager.BookRepo.GetBookById(id, trackChanges);

            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            entity.Title = book.Title;
            entity.Price = book.Price;

            _manager.BookRepo.UpdateOneBook(entity);
            _manager.Save();
        }
    }
}
