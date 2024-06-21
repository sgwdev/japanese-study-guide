using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> source, ISpecification<T> specification) where T : BaseEntity
        {
            var query = specification.Includes.Aggregate(
                source, (current, include) => include(current)
            );

            if (specification.IsRandomized)
            {
                query = query.OrderBy(x => EF.Functions.Random());
            }

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.Take > 0)
            {
                query = query.Take(specification.Take);
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            return query;
        }
    }
}
