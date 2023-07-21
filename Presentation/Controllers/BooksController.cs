﻿using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttribute))]
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
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
        {
            var pagedResult = await _manager.BookService.GetAllBooksAsync(bookParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.books); //200
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookByIdAync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager.BookService.GetBookByIdAsync(id, false);
            return Ok(book); //200
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            //ValidationFilterAttribute'ü hazırlayarak bu sorgulara ait ihtiyacımızı karşıladık.
            //if (bookDto is null)
            //    return BadRequest(); //400
            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState); //422

            //Kitabı repoya ekler
            var book = await _manager.BookService.CreateBookAsync(bookDto);

            return StatusCode(201, bookDto); //201
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBookByIdAsync([FromRoute] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            await _manager.BookService.UpdateBookAsync(id, false, bookDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBookByIdAsync([FromRoute] int id)
        {
            await _manager.BookService.DeleteBookAsync(id, false);
            return NoContent(); //204

        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchBookByIdAsync([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> patchBook)
        {
            if (patchBook is null)
                return BadRequest();

            var result = await _manager.BookService.GetBookForPatchAsync(id, false);


            patchBook.ApplyTo(result.bookDtoForUpdate,ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

            return NoContent(); //204
        }

    }
}
