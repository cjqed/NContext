﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceResponseIQueryableExtensions.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using NContext.Common;

    /// <summary>
    /// Defines extension methods for <see cref="IQueryable{T}"/> yielding a new <see cref="IServiceResponse{T}"/>.
    /// </summary>
    public static class IServiceResponseIQueryableExtensions
    {
        /// <summary>
        /// Returns an <see cref="IServiceResponse{T}"/> with the first element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to return the first element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IServiceResponse{T} with the first element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IServiceResponse<T> FirstResponse<T>(this IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ErrorResponse<T>(
                        new Error(
                            (Int32)HttpStatusCode.InternalServerError,
                            "IResponseTransferObjectIQueryableExtensions_FirstResponse_NoMatch",
                            new[] { "Enumerable is empty." }));
                }

                return new DataResponse<T>(enumerator.Current);
            }
        }

        /// <summary>
        /// Returns an <see cref="IServiceResponse{T}"/> with the single, specific element of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/> to return the single element of.</param>
        /// <param name="predicate">An optional function to test each element for a condition.</param>
        /// <returns>IServiceResponse{T} with the single element in the sequence that passes the test in the (optional) predicate function.</returns>
        public static IServiceResponse<T> SingleResponse<T>(this IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            using (var enumerator = GetEnumerator(queryable, predicate))
            {
                if (!enumerator.MoveNext())
                {
                    return new ErrorResponse<T>(
                        new Error(
                            (Int32)HttpStatusCode.InternalServerError,
                            "IResponseTransferObjectIQueryableExtensions_SingleResponse_NoMatch",
                            new[] { "Enumerable is empty." }));
                }

                T current = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return new DataResponse<T>(current);
                }
            }
            
            return new ErrorResponse<T>(
                new Error(
                    (Int32)HttpStatusCode.InternalServerError,
                    "IResponseTransferObjectIQueryableExtensions_SingleResponse_MoreThanOneMatch",
                    new[] { "Enumerable has more than one matched entry." }));
        }

        private static IEnumerator<T> GetEnumerator<T>(IQueryable<T> queryable, Expression<Func<T, Boolean>> predicate = null)
        {
            return (predicate == null) ? queryable.GetEnumerator() : queryable.Where(predicate).GetEnumerator();
        }
    }
}