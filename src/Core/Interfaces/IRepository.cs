using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        public T GetById(int id);

        public T Get(ISpecification<T> specification);

        public List<T> GetList();

        public List<T> GetList(ISpecification<T> specification);

        public void Add(T entity);

        public void Update(T entity);

        public void Save();
    }
}
