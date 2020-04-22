using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Subscrib_er.Services.Interface
{
    public interface IRepository<T> where T: class
    {
        T Insert(T entity);
        void Update(T entity);
        void Delete(Guid id);
        bool DeletePackagePayment(string id);
        ICollection<T> GetAll();
        T GetUserById(string id);
        T GetEntityById(Guid id);
        public ICollection<T> FindAll(Expression<Func<T, bool>> match);
    }
}
