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

            try
            {
                var book = _manager.BookService.GetBookById(id, false);

                if (book is null)
                    return NotFound(); //404

                return Ok(book); //200

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }


        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            try
            {
                //Kitabı repoya ekler
                _manager.BookService.CreateBook(book);

                return StatusCode(201, book); //201

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBookById([FromRoute] int id, [FromBody] Book book)
        {
            try
            {
                _manager.BookService.UpdateBook(id, true, book);
                return NoContent();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBookById([FromRoute] int id)
        {

            try
            {
                _manager.BookService.DeleteBook(id, false);
                return NoContent(); //204
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PatchBookById([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> patchBook)
        {
            try
            {
                var entity = _manager.BookService.GetBookById(id, true);

                if (entity is null)
                    return NotFound(); //404

                patchBook.ApplyTo(entity);
                _manager.BookService.UpdateBook(id, true, entity);
                return NoContent(); //204

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
