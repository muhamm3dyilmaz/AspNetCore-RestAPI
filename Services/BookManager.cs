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
using Entities.RequestFeatures;
using System.Dynamic;
using Entities.LinkModels;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        //Hateoas için dataShaperı kaldırıp bookLinksi ekledik
        private readonly IBookLinks _bookLinks;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IBookLinks bookLinks)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _bookLinks = bookLinks;
        }

        public async Task<BookDto> CreateBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.BookRepo.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteBookAsync(int id, bool trackChanges)
        {
            var entity = await GetBookByIdAndCheckExists(id, trackChanges);

            _manager.BookRepo.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        //Hateoas ile içeriği yeniledik
        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, 
            bool trackChanges)
        {
            if (!linkParameters.BookParameters.ValidPriceRange)
                throw new PriceOutOfRangeBadRequestException();

            var booksWithMetaData = await _manager.BookRepo.GetAllBooksAsync(linkParameters.BookParameters, trackChanges);

            //kitapları mapper ile mapleyip IEnumerable yani foreach ile döndürülmesini sağladık
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

            var links = _bookLinks.TryGenerateLinks(booksDto, linkParameters.BookParameters.Fields, linkParameters.HttpContext);

            return (linkResponse: links, metaData: booksWithMetaData.MetaData);
        }

        //API V2 için
        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            var books = await _manager.BookRepo.GetAllBooksAsync(trackChanges);
            return books;
        }

        public async Task<BookDto> GetBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetBookByIdAndCheckExists(id,trackChanges);

            return _mapper.Map<BookDto>(book);
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await _manager.BookRepo.GetBookByIdAsync(id, trackChanges);

            if(book is null)
                throw new BookNotFoundException(id);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            //tuple ifadesini kullandık
            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task UpdateBookAsync(int id, bool trackChanges, BookDtoForUpdate bookDto)
        {
            var entity = await GetBookByIdAndCheckExists(id, trackChanges);

            //Mapper
            //entity.Title = book.Title;
            //entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepo.UpdateOneBook(entity);
            await _manager.SaveAsync();
        }
    
        private async Task<Book> GetBookByIdAndCheckExists(int id, bool trackChanges)
        {
            var entity = await _manager.BookRepo.GetBookByIdAsync(id, trackChanges);

            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            return entity;
        }
    }
}
