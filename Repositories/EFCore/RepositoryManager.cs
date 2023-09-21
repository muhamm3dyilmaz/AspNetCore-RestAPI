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

        //Lazy LoadingBookRepository kullanıldığında new işlemi yapar flutterdaki getx obx gibi
        private readonly Lazy<IBookRepository> _bookRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;

            _bookRepository = new Lazy<IBookRepository>(() => 
                              new BookRepository(_repositoryContext));

            _categoryRepository = new Lazy<ICategoryRepository>(() =>
                                  new CategoryRepository(_repositoryContext));
        }

        //New işlemi yapılan BookRepository i yüklemesini yapar
        public IBookRepository BookRepo => _bookRepository.Value;

        public ICategoryRepository CategoryRepo => _categoryRepository.Value;

        /*SaveChanges ı ayrı yapmamızın sebebi IBookRepository ve BookRepository de sadece
        CRUD işlemlerini yapabilmemizdir bu yüzden bir repo manager e ihtiyaç duyarız */
        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
