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

            if(specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            return query;
        }
    }
}
