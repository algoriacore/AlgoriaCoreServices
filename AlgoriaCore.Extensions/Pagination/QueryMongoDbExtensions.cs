using AlgoriaCore.Domain.Interfaces.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AlgoriaCore.Extensions.Pagination
{
    public static class QueryMongoDbExtensions
    {
        #region IMongoQueryable

        public static IMongoQueryable<T> PageBy<T>(this IMongoQueryable<T> query, IPagedResult pagedDto)
        {
            return query.PageBy(pagedDto.PageNumber.Value, pagedDto.PageSize.Value);
        }

        public static IMongoQueryable<T> PageBy<T>(this IMongoQueryable<T> query, int pageNumber, int pageSize)
        {
            int skip = (pageSize * pageNumber) - pageSize;

            return query.Skip(skip).Take(pageSize);
        }

        public static IMongoQueryable<T> OrderBy<T>(this IMongoQueryable<T> query, string sorting)
        {
            if (!sorting.IsNullOrEmpty())
            {
                if (!sorting.ToLower().Trim().EndsWith("asc") && !sorting.ToLower().Trim().EndsWith("desc"))
                {
                    query = (IMongoQueryable<T>)query.OrderBy(sorting + " asc", null, null);
                }
                else
                {
                    query = (IMongoQueryable<T>)query.OrderBy(sorting, null, null);
                }
            }

            return query;
        }

        public static IMongoQueryable<T> WhereIf<T>(this IMongoQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        #endregion

        #region IFindFluent

        public static IFindFluent<BsonDocument, BsonDocument> PageBy<BsonDocument>(this IFindFluent<BsonDocument, BsonDocument> query, IPagedResult pagedDto)
        {
            return query.PageBy(pagedDto.PageNumber.Value, pagedDto.PageSize.Value);
        }

        public static IFindFluent<BsonDocument, BsonDocument> PageBy<BsonDocument>(this IFindFluent<BsonDocument, BsonDocument> query, int pageNumber, int pageSize)
        {
            int skip = (pageSize * pageNumber) - pageSize;

            return query.Skip(skip).Limit(pageSize);
        }

        public static IFindFluent<BsonDocument, BsonDocument> OrderBy<BsonDocument>(this IFindFluent<BsonDocument, BsonDocument> query, string sorting)
        {
            if (!sorting.IsNullOrEmpty())
            {
                string[] parts = sorting.Split(",", StringSplitOptions.TrimEntries);
                List<string> sortsJSON = new List<string>();

                foreach (string part in parts)
                {
                    if (part.ToLower().EndsWith(" desc"))
                    {
                        sortsJSON.Add(string.Format("\"{0}\": -1", part.Substring(0, part.Length - 5)));
                    }
                    else if (part.ToLower().EndsWith(" asc"))
                    {
                        sortsJSON.Add(string.Format("\"{0}\": 1", part.Substring(0, part.Length - 4)));
                    }
                    else
                    {
                        sortsJSON.Add(string.Format("\"{0}\": 1", part));
                    }
                }

                query = query.Sort(MongoDB.Bson.BsonDocument.Parse("{" + string.Join(", ", sortsJSON) + "}"));
            }

            return query;
        }


        #endregion

        #region IAggregateFluent

        public static IAggregateFluent<BsonDocument> PageBy<BsonDocument>(this IAggregateFluent<BsonDocument> query, IPagedResult pagedDto)
        {
            return query.PageBy(pagedDto.PageNumber.Value, pagedDto.PageSize.Value);
        }

        public static IAggregateFluent<BsonDocument> PageBy<BsonDocument>(this IAggregateFluent<BsonDocument> query, int pageNumber, int pageSize)
        {
            int skip = (pageSize * pageNumber) - pageSize;

            return query.Skip(skip).Limit(pageSize);
        }

        public static IAggregateFluent<BsonDocument> OrderBy<BsonDocument>(this IAggregateFluent<BsonDocument> query, string sorting)
        {
            if (!sorting.IsNullOrEmpty())
            {
                string[] parts = sorting.Split(",", StringSplitOptions.TrimEntries);
                SortDefinitionBuilder<BsonDocument> sortBuilder = Builders<BsonDocument>.Sort;

                foreach (string part in parts)
                {
                    if (part.ToLower().EndsWith(" desc"))
                    {
                        query = query.Sort(sortBuilder.Descending(part.Substring(0, part.Length - 5)));
                    }
                    else if (part.ToLower().EndsWith(" asc"))
                    {
                        query = query.Sort(sortBuilder.Ascending(part.Substring(0, part.Length - 4)));
                    }
                    else
                    {
                        query = query.Sort(sortBuilder.Ascending(part));
                    }
                }
            }

            return query;
        }


        #endregion
    }
}
