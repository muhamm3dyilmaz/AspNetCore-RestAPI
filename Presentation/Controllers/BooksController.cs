using Entities.DataTransferObjects;
using System.Text.Json;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using Entities.Exceptions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    //[ApiVersion("1.0")]
    //[Authorize] //endpointleri yetkisiz işlemlere karşı korur (401 unauthorized)
    [ApiController]
    [Route("api/books")]
    [ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/{v:apiversion}/books")] URL bazlı versiyonlama için
    [ServiceFilter(typeof(LogFilterAttribute))]
    //[ResponseCache(CacheProfileName = "5mins")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        /*[Authorize(Roles = "User")] *///rol tabanlı yetkilendirme
        [HttpHead]
        [HttpGet(Name = "GetAllBooksAsync")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        //[ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
        {
            var linkParameters = new LinkParameters()
            {
                BookParameters = bookParameters,
                HttpContext = HttpContext
            };

            var result = await _manager.BookService.GetAllBooksAsync(linkParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLink ?
                Ok(result.linkResponse.LinkedEntities) :
                Ok(result.linkResponse.ShapedEntities);
        }

        //[Authorize(Roles = "Editor")] //rol tabanlı yetkilendirme
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookByIdAync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager.BookService.GetBookByIdAsync(id, false);
            return Ok(book); //200
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksWithDetailsAsync()
        {
            return Ok(await _manager.BookService.GetAllBooksWithDetailsAsync(false));
        }

        /*[Authorize(Roles = "Admin")]*/ //rol tabanlı yetkilendirme
        [HttpPost(Name = "CreateBookAsync")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            var book = await _manager.BookService.CreateBookAsync(bookDto);

            return StatusCode(201, bookDto); //201
        }

        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
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

        //İstediğimiz işlemlerin kulanımına izin verdik
        [HttpOptions]
        public IActionResult GetBookOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, DELETE, PATCH, OPTIONS, HEAD");
            return Ok();
        }
    }
}
