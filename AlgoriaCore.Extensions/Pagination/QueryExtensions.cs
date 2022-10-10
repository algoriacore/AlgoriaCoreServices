using AlgoriaCore.Domain.Interfaces.Entity;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AlgoriaCore.Extensions.Pagination
{
    public static class QueryExtensions
    {
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedResult pagedDto)
        {
            return query.PageBy(pagedDto.PageNumber.Value, pagedDto.PageSize.Value);
        }

        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            int skip = (pageSize * pageNumber) - pageSize;

            return query.Skip(skip).Take(pageSize);
        }

        public static IQueryable<T> PageByIf<T>(this IQueryable<T> query, bool condition, IPagedResult pagedDto)
        {
            return condition ? query.PageBy(pagedDto) : query;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sorting)
        {
            if (!sorting.IsNullOrEmpty())
            {
                if (!sorting.ToLower().Trim().EndsWith("asc") && !sorting.ToLower().Trim().EndsWith("desc"))
                {
                    query = query.OrderBy(sorting + " asc", null, null);
                }
                else
                {
                    query = query.OrderBy(sorting, null, null);
                }
            }

            return query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }
    }
}
