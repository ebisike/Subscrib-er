using Subscrib_er.Data;
using Subscrib_er.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Subscrib_er.Services.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SubscribDbContext subscribDb;

        public Repository(SubscribDbContext subscribDb)
        {
            this.subscribDb = subscribDb;
        }

        public void Delete(Guid id)
        {
            var entity = subscribDb.Set<T>().Find(id);
            if (entity != null)
            {
                subscribDb.Set<T>().Remove(entity);
                subscribDb.SaveChanges();
            }
        }

        public bool DeletePackagePayment(string id)
        {
            var entity = subscribDb.Set<T>().Find(id);
            if (entity != null)
            {
                subscribDb.Set<T>().Remove(entity);
                subscribDb.SaveChanges();
                return true;
            }
            return false;
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return subscribDb.Set<T>().Where(match).ToList();
        }

        public ICollection<T> GetAll()
        {
            return subscribDb.Set<T>().ToList();
        }

        public T GetEntityById(Guid id)
        {
            return subscribDb.Set<T>().Find(id);
        }

        public T GetUserById(string id)
        {
            return subscribDb.Set<T>().Find(id);
        }

        public T Insert(T entity)
        {
            subscribDb.Set<T>().Add(entity);
            subscribDb.SaveChanges();
            return entity;
        }

        public void Update(T entity)
        {
            subscribDb.Set<T>().Update(entity);
            subscribDb.SaveChanges();
        }
    }
}
