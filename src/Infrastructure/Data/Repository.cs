using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Extensions;
using Infrastructure.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        public Repository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Where(e => e.Id == id).SingleOrDefault();
        }

        public T Get(ISpecification<T> specification)
        {
            return _dbContext.Set<T>().AsQueryable().ApplySpecification(specification).SingleOrDefault();
        }

        public List<T> GetList()
        {
            return _dbContext.Set<T>().ToList();
        }

        public List<T> GetList(ISpecification<T> specification)
        {
            return _dbContext.Set<T>().AsQueryable().ApplySpecification(specification).ToList();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
