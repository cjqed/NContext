﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EfServiceResponse.cs">
//   Copyright (c) 2012
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
// 
// <summary>
//   Defines an entity framework ServiceResponse<T> adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

using NContext.Dto;
using NContext.ErrorHandling;

namespace NContext.Extensions.EntityFramework
{
    /// <summary>
    /// Defines an Entity Framework <see cref="ServiceResponse{T}"/> adapter.
    /// </summary>
    public class EfServiceResponse<T> : ServiceResponse<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <remarks></remarks>
        public EfServiceResponse(ErrorBase error)
            : this(new List<ErrorBase> { error })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <remarks></remarks>
        public EfServiceResponse(IEnumerable<ErrorBase> errors)
            : base(TranslateErrorBaseToErrorCollection(errors))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <remarks></remarks>
        public EfServiceResponse(DbEntityValidationResult validationResult)
            : this(new List<DbEntityValidationResult> { validationResult })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <remarks></remarks>
        public EfServiceResponse(IEnumerable<DbEntityValidationResult> validationResults)
            : base(TranslateDbEntityValidationResultsToValidationErrors(validationResults))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public EfServiceResponse(T data)
            : base(data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        public EfServiceResponse(IEnumerable<T> data)
            : base(data)
        {
        }

        #endregion

        #region Methods

        private static IEnumerable<Error> TranslateErrorBaseToErrorCollection(IEnumerable<ErrorBase> errors)
        {
            return errors.ToMaybe()
                         .Bind(e => e.Select(error => new Error(error.ErrorType.Name, new List<String> { error.Message }, error.HttpStatusCode.ToString())).ToMaybe())
                         .FromMaybe(Enumerable.Empty<Error>());
        }

        private static IEnumerable<ValidationError> TranslateDbEntityValidationResultsToValidationErrors(IEnumerable<DbEntityValidationResult> validationResults)
        {
            return validationResults.ToMaybe()
                                    .Bind(results => 
                                          results.Select(validationResult => 
                                                         new ValidationError(validationResult.Entry.Entity.GetType(),
                                                                             validationResult.ValidationErrors
                                                                                             .Select(validationError => validationError.ErrorMessage))).ToMaybe())
                                    .FromMaybe(Enumerable.Empty<ValidationError>());
        }

        #endregion
    }
}