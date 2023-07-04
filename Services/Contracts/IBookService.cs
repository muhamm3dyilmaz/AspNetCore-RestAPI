using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        BookDto GetBookById(int id, bool trackChanges);
        BookDto CreateBook(BookDtoForInsertion book);
        void UpdateBook(int id, bool trackChanges, BookDtoForUpdate bookDto);
        void DeleteBook(int id, bool trackChanges);
        //tuple ifadesi oluşturduk
        (BookDtoForUpdate bookDtoForUpdate, Book book) GetBookForPatch(int id, bool trackChanges);
        void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book);
    }
}
