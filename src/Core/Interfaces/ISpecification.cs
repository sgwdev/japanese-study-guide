using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Func<IQueryable<T>, IQueryable<T>>> Includes { get; }
        
        public int Take { get; }
        public Expression<Func<T, int>> OrderBy { get; }
        public bool IsRandomized { get; }
        
    }
}
