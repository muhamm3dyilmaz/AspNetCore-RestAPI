using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        //Hateoas için getAll içeriğini yeniledik
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, 
            bool trackChanges);
        Task<BookDto> GetBookByIdAsync(int id, bool trackChanges);
        Task<BookDto> CreateBookAsync(BookDtoForInsertion book);
        Task UpdateBookAsync(int id, bool trackChanges, BookDtoForUpdate bookDto);
        Task DeleteBookAsync(int id, bool trackChanges);
        //tuple ifadesi oluşturduk
        Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetBookForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);

        //////////////////// API V2 İÇİN EKLEDİKLERİMİZ ///////////////////////
        Task<List<Book>> GetAllBooksAsync(bool trackChanges);
    }
}
