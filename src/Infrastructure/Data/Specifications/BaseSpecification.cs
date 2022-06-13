using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; protected set; }
        public List<Func<IQueryable<T>, IQueryable<T>>> Includes { get; } = new List<Func<IQueryable<T>, IQueryable<T>>>();

        public BaseSpecification()
        {

        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected virtual void AddInclude(Func<IQueryable<T>, IQueryable<T>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}
