using Entities.Models;
using Services.Contracts;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Exceptions;
using AutoMapper;
using Entities.DataTransferObjects;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public BookDto CreateBook(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.BookRepo.CreateOneBook(entity);
            _manager.Save();
            return _mapper.Map<BookDto>(entity);
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

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _manager.BookRepo.GetAllBooks(trackChanges);
            //kitapları mapper ile mapleyip IEnumerable yani foreach ile döndürülmesini sağladık
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public BookDto GetBookById(int id, bool trackChanges)
        {
            var book = _manager.BookRepo.GetBookById(id,trackChanges);

            //Entities katmanında yazdığımız exception kullanıyoruz
            if (book is null)
            {
                throw new BookNotFoundException(id);
            }

            return _mapper.Map<BookDto>(book);
        }

        public (BookDtoForUpdate bookDtoForUpdate, Book book) GetBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.BookRepo.GetBookById(id, trackChanges);

            if(book is null)
                throw new BookNotFoundException(id);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            //tuple ifadesini kullandık
            return (bookDtoForUpdate, book);
        }

        public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            _manager.Save();
        }

        public void UpdateBook(int id, bool trackChanges, BookDtoForUpdate bookDto)
        {
            var entity = _manager.BookRepo.GetBookById(id, trackChanges);

            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            //Mapper
            //entity.Title = book.Title;
            //entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepo.UpdateOneBook(entity);
            _manager.Save();
        }
    }
}
