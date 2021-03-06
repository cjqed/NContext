﻿namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NContext.Data.Persistence;
    using NContext.Data.Specifications;

    /// <summary>
    /// Defines extension methods for IEnumerable.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Queries the context based on the provided specification and returns results that
        /// are only satisfied by the specification.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="specification">
        /// A <see cref="SpecificationBase{TEntity}" /> instance used to filter results that only satisfy the specification.
        /// </param>
        /// <returns>A <see cref="IQueryable{TEntity}" /> that can be used to enumerate over the results
        /// of the query.</returns>
        /// <paramref name="entities" />
        public static IEnumerable<TEntity> AllMatching<TEntity>(this IEnumerable<TEntity> entities, SpecificationBase<TEntity> specification)
            where TEntity : class, IEntity
        {
            return entities.AsQueryable().Where(specification.IsSatisfiedBy());
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> getKey)
        {
            var hashSet = new HashSet<TKey>();

            return from item in source 
                   let key = getKey(item) 
                   where hashSet.Add(key) 
                   select item;
        }

        /// <summary>
        /// ForEach extension that enumerates over all items in an <see cref="IEnumerable{T}.Collections.Generic.IEnumerable{T}"/> and executes 
        /// an action.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="collection">The enumerable instance that this extension operates on.</param>
        /// <param name="action">The action executed for each iten in the enumerable.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.GetEnumerator().ForEach(action);
        }

        /// <summary>
        /// ForEach extension that enumerates over all items in an <see cref="IEnumerator{T}.Collections.Generic.IEnumerator{T}"/> and executes 
        /// an action.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="enumerator">The enumerator instance that this extension operates on.</param>
        /// <param name="action">The action executed for each iten in the enumerable.</param>
        public static void ForEach<T>(this IEnumerator<T> enumerator, Action<T> action)
        {
            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
        }

        /// <summary>
        /// ForEach extension that enumerates over a enumerable enumerator and attempts to execute 
        /// the provided action delegate and it the action throws an exception, continues enumerating.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="collection">The IEnumerable instance that ths extension operates on.</param>
        /// <param name="action">The action excecuted for each item in the enumerable.</param>
        public static void TryForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.GetEnumerator().TryForEach(action);
        }

        /// <summary>
        /// ForEach extension that enumerates over an enumerator and attempts to execute the provided
        /// action delegate and if the action throws an exception, continues executing.
        /// </summary>
        /// <typeparam name="T">The type that this extension is applicable for.</typeparam>
        /// <param name="enumerator">The IEnumerator instace</param>
        /// <param name="action">The action executed for each item in the enumerator.</param>
        public static void TryForEach<T>(this IEnumerator<T> enumerator, Action<T> action)
        {
            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    try
                    {
                        action(enumerator.Current);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}