﻿namespace NContext.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Defines a generic data-transfer-object for containing arbitrary data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResponse<T> : ServiceResponse<T>
    {
        private readonly T _Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DataResponse(T data)
        {
            var materializedData = MaterializeDataIfNeeded(data);
            _Data = (materializedData == null) ? default(T) : materializedData;
        }

        /// <summary>
        /// Gets whether the instance is left.
        /// </summary>
        /// <value>The is left.</value>
        public override Boolean IsLeft
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the left value.
        /// </summary>
        /// <returns>Error.</returns>
        public override Error GetLeft()
        {
            return null;
        }

        /// <summary>
        /// Gets the right value, <see cref="Data"/>.
        /// </summary>
        /// <returns>T.</returns>
        public override T GetRight()
        {
            return _Data;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public override T Data
        {
            get { return _Data; }
        }

        private static T MaterializeDataIfNeeded(T data)
        {
            if (typeof(T).IsValueType || data == null)
            {
                return data;
            }

            var dataType = data.GetType();
            if (!(data is IEnumerable) ||
                !dataType.IsGenericType ||
                IsDictionary(dataType))
            {
                return data;
            }

            if (!IsQueryable(dataType) && !dataType.IsNestedPrivate)
            {
                return data;
            }

            // Get the last generic argument.
            // .NET has several internal iterable types in LINQ that have multiple generic
            // arguments.  The last is reserved for the actual type used for projection.
            // ex. WhereSelectArrayIterator, WhereSelectEnumerableIterator, WhereSelectListIterator
            var genericType = dataType.GetGenericArguments().Last();
            if (dataType.GetGenericTypeDefinition() == typeof(Collection<>))
            {
                var collectionType = typeof(Collection<>).MakeGenericType(genericType);
                return (T)collectionType.CreateInstance(data);
            }

            var listType = typeof(List<>).MakeGenericType(genericType);
            return (T)listType.CreateInstance(data);
        }

        private static Boolean IsDictionary(Type type)
        {
            if (type == null) return false;

            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                   type.GetInterfaces()
                       .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private static Boolean IsQueryable(Type type)
        {
            if (type == null) return false;

            return
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IQueryable<>)) ||
                type.GetInterfaces()
                    .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IQueryable<>));
        }
    }
}