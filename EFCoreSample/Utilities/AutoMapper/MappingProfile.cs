using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace EFCoreSample.Utilities.AutoMapper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<BookDtoForInsertion, Book>();

            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
