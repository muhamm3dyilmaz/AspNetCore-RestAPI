using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    //Kendi Repo interfacemizi yazmak temel CRUD işlemlerini tüm projede kullanmaya olanak sağlar
    public interface IRepositoryBase<T>
    {
        //CRUD işlemleri
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
