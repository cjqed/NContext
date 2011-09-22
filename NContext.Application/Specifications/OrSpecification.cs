﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrSpecification.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines a composite specification for OR-logic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace NContext.Application.Domain
{
    /// <summary>
    /// Defines a composite specification for OR-logic.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification</typeparam>
    public sealed class OrSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class, IEntity
    {
        #region Members

        private readonly SpecificationBase<TEntity> _LeftSideSpecification;

        private readonly SpecificationBase<TEntity> _RightSideSpecification;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrSpecification&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        /// <remarks></remarks>
        public OrSpecification(SpecificationBase<TEntity> leftSide, SpecificationBase<TEntity> rightSide)
        {
            if (leftSide == null)
            {
                throw new ArgumentNullException("leftSide");
            }

            if (rightSide == null)
            {
                throw new ArgumentNullException("rightSide");
            }

            _LeftSideSpecification = leftSide;
            _RightSideSpecification = rightSide;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Left side specification
        /// </summary>
        public override SpecificationBase<TEntity> LeftSideSpecification
        {
            get { return _LeftSideSpecification; }
        }

        /// <summary>
        /// Righ side specification
        /// </summary>
        public override SpecificationBase<TEntity> RightSideSpecification
        {
            get { return _RightSideSpecification; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfied()
        {
            Expression<Func<TEntity, Boolean>> left = _LeftSideSpecification.IsSatisfied();
            Expression<Func<TEntity, Boolean>> right = _RightSideSpecification.IsSatisfied();

            ParameterExpression param = left.Parameters[0];
            if (ReferenceEquals(param, right.Parameters[0]))
            {
                return Expression.Lambda<Func<TEntity, Boolean>>(Expression.OrElse(left.Body, right.Body), param);
            }

            return Expression.Lambda<Func<TEntity, Boolean>>(Expression.OrElse(left.Body, Expression.Invoke(right, param)), param);
        }

        #endregion
    }
}