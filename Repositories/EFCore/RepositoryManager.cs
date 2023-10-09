using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    //Birden fazla reponun olacağını ön görerek Unit of Work Pattern ini kullandık 
    public class RepositoryManager : IRepositoryManager
    {
        //DbContext ten kalıtım alıp kendi oluşturduğumuz RepositoryContext
        private readonly RepositoryContext _repositoryContext;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        //Newleme işlemi constructor içinde yapıldı -> önceki hali Lazy<>
        public RepositoryManager(RepositoryContext repositoryContext, IBookRepository bookRepository, 
            ICategoryRepository categoryRepository)
        {
            _repositoryContext = repositoryContext;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        //New işlemi yapılan BookRepository i yüklemesini yapar
        public IBookRepository BookRepo => _bookRepository;

        public ICategoryRepository CategoryRepo => _categoryRepository;

        /*SaveChanges ı ayrı yapmamızın sebebi IBookRepository ve BookRepository de sadece
        CRUD işlemlerini yapabilmemizdir bu yüzden bir repo manager e ihtiyaç duyarız */
        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
