using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Presentation.Controllers
{
    
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {

            try
            {
                var books = _manager.BookService.GetAllBooks(false);
                return Ok(books); //200
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById([FromRoute(Name = "id")] int id)
        {
            var book = _manager.BookService.GetBookById(id, false);
            return Ok(book); //200
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookDtoForInsertion bookDto)
        {
            if (bookDto is null)
                return BadRequest(); //400

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            //Kitabı repoya ekler
           var book = _manager.BookService.CreateBook(bookDto);

            return StatusCode(201, bookDto); //201
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBookById([FromRoute] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            _manager.BookService.UpdateBook(id, false, bookDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBookById([FromRoute] int id)
        {
            _manager.BookService.DeleteBook(id, false);
            return NoContent(); //204

        }

        [HttpPatch("{id:int}")]
        public IActionResult PatchBookById([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> patchBook)
        {
            if (patchBook is null)
                return BadRequest();

            var result = _manager.BookService.GetBookForPatch(id, false);


            patchBook.ApplyTo(result.bookDtoForUpdate,ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _manager.BookService.SaveChangesForPatch(result.bookDtoForUpdate, result.book);

            return NoContent(); //204
        }

    }
}
