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
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (book is null)
                return BadRequest(); //400

            //Kitabı repoya ekler
            _manager.BookService.CreateBook(book);

            return StatusCode(201, book); //201
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBookById([FromRoute] int id, [FromBody] Book book)
        {
            _manager.BookService.UpdateBook(id, true, book);
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
            [FromBody] JsonPatchDocument<Book> patchBook)
        {
            var entity = _manager.BookService.GetBookById(id, true);

            if (entity is null)
                return NotFound(); //404

            patchBook.ApplyTo(entity);
            _manager.BookService.UpdateBook(id, true, entity);
            return NoContent(); //204
        }

    }
}
