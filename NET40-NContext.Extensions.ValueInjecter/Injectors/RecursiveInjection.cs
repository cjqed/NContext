namespace NContext.Extensions.ValueInjecter.Injectors
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using Omu.ValueInjecter;

    /// <summary>
    /// Recursively translates classes.
    /// </summary>
    public sealed class RecursiveInjection : ConventionInjection
    {
        /// <summary>
        /// Holds on to constructors
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Delegate> ConstructorCache = new ConcurrentDictionary<Type, Delegate>();

        /// <summary>
        /// Cache for the casting method.
        /// </summary>
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, MethodInfo> CastingCache = new ConcurrentDictionary<Tuple<Type, Type>, MethodInfo>();

        protected override bool Match(ConventionInfo c)
        {
            var clause1 = c.SourceProp.Name == c.TargetProp.Name;

            var clause2 = c.SourceProp.Value != null;

            return clause1 && clause2;
        }

        protected override object SetValue(ConventionInfo c)
        {
            if (c.SourceProp.Type.IsValueType || c.SourceProp.Type == typeof(string))
            {
                return c.SourceProp.Value;
            }

            if(!c.SourceProp.Type.IsGenericType || (c.SourceProp.Type.IsGenericType && !c.SourceProp.Value.GetType().GetGenericTypeDefinition().GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
            {
                var constructor = ConstructorCache.GetOrAdd(c.TargetProp.Type, type =>
                    {
                        var newConstructor = type.GetConstructor(new Type[0]);
                        return Expression.Lambda(Expression.New(newConstructor)).Compile();
                    });

                var newClass = constructor.DynamicInvoke();
                return newClass.InjectFrom<RecursiveInjection>(c.SourceProp.Value);
            }

            if (c.SourceProp.Value.GetType().GetGenericTypeDefinition().GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                if (c.SourceProp.Value == null)
                {
                    return null;
                }

                var concreteSourceGenericProp = c.SourceProp.Type.GetGenericArguments().Single();
                var concreteTargetGenericProp = c.TargetProp.Type.GetGenericArguments().Single();

                var thing =
                    c.SourceProp.Value.GetType().GetGenericTypeDefinition().MakeGenericType(concreteTargetGenericProp);

                var constructor = ConstructorCache.GetOrAdd(thing, type =>
                    {
                        var genericTypeConstructor = typeof(IEnumerable<>).MakeGenericType(concreteTargetGenericProp);
                        var newConstructor = thing.GetConstructor(new[] { genericTypeConstructor });
                        var parameter = Expression.Parameter(genericTypeConstructor);
                        return Expression.Lambda(Expression.New(newConstructor, parameter), parameter).Compile();
                    });


                var ultraCastMethod = this.GetType().GetMethod("UltraCast");

                var convertedEnumerableMethod =
                    CastingCache.GetOrAdd(
                        new Tuple<Type, Type>(concreteSourceGenericProp, concreteTargetGenericProp),
                        type => ultraCastMethod.MakeGenericMethod(concreteSourceGenericProp, concreteTargetGenericProp));

                var converted = convertedEnumerableMethod.Invoke(this, new[] { c.SourceProp.Value, null });


                if (constructor != null)
                {
                    return constructor.DynamicInvoke(new[] { converted });
                }
            }

            return null;
        }

        public static IEnumerable<TB> UltraCast<T, TB>(IEnumerable<T> input, TB prototype)
        {
            return
                input.Select(
                    x =>
                        {
                            var constructor = ConstructorCache.GetOrAdd(typeof(TB), type =>
                                {
                                    var newConstructor = type.GetConstructor(new Type[0]);
                                    return Expression.Lambda<Func<TB>>(Expression.New(newConstructor)).Compile();
                                });
                            var newInstance = constructor.DynamicInvoke();
                            return (TB)newInstance.InjectFrom<RecursiveInjection>(x);
                        });
        }
    }
}