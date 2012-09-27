﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkExtensions.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;

    using NContext.Common;
    using NContext.Data.Persistence;

    /// <summary>
    /// Defines extension methods for Entity Framework.
    /// </summary>
    /// <remarks></remarks>
    public static class EntityFrameworkExtensions
    {
        /// <summary>
        /// Determines whether all the validation results are valid.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <returns><c>True</c> if all validation results are valid, else <c>false</c>.</returns>
        /// <remarks></remarks>
        public static Boolean AreValid(this IEnumerable<DbEntityValidationResult> validationResults)
        {
            return validationResults.All(validationResult => validationResult.IsValid);
        }

        /// <summary>
        /// Returns a new <see cref="IResponseTransferObject{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>IResponseTransferObject{TEntity}.</returns>
        public static IResponseTransferObject<TEntity> ToServiceResponse<TEntity>(this TEntity entity)
            where TEntity : IEntity
        {
            return new EfServiceResponse<TEntity>(entity);
        }
    }
}