using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookLinks : IBookLinks
    {
        //LinkGenerator abstract varolan bir class
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<BookDto> _dataShaper;

        public BookLinks(LinkGenerator linkGenerator, IDataShaper<BookDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext)
        {
            var shapedBooks = ShapedData(booksDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedBooks(booksDto, fields, httpContext, shapedBooks);

            return ReturnShapedBooks(shapedBooks);
        }

        //Linked book dataları döndürmesi için fonksiyon yazdık ve yukarıda kullandık (ayrıyeten link oluşturma fonksiyonlarıda burda)
        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext, 
            List<Entity> shapedBooks)
        {
            var bookDtoList = booksDto.ToList();

            for(int index = 0; index < bookDtoList.Count();  index++)
            {
                var bookLinks = CreateForBook(httpContext, bookDtoList[index]);
                shapedBooks[index].Add("Links", bookLinks);
            }

            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            CreateForBooks(httpContext, bookCollection);
            return new LinkResponse { HasLink = true, LinkedEntities = bookCollection };
        }

        //kitaplar için link oluşturur (ör: en çok okunan en çok satılan kitaplar için kullanılabilir)
        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext, 
            LinkCollectionWrapper<Entity> bookCollectionWrapper)
        {
            bookCollectionWrapper.Links.Add(new Link() 
            {
                Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                Rel = "self",
                Method = "GET"
            });

            return bookCollectionWrapper;
        }

        //Kitap için link oluşturur
        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}" + $"/{bookDto.Id}",
                    Rel = "self",
                    Method = "GET",
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                    Rel = "create",
                    Method = "PUT",
                },
            };

            return links;
        }

        //Shaped book dataları döndürmesi için fonksiyon yazdık ve yukarıda kullandık
        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse() { ShapedEntities = shapedBooks };
        }

        //Linklerin oluşturulmasına yönelik fonksiyon yazdık ve yukarıda kullandık
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        //Dataların shape'lenmesi için fonksiyon yazdık ve yukarıda kullandık
        private List<Entity> ShapedData(IEnumerable<BookDto> booksDto, string fields)
        {
            return _dataShaper.ShapeData(booksDto, fields).Select(b => b.Entity).ToList();
        }
    }
}
