using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeMonkeys
{
    /// <summary>
    /// Expression based Activator implementation.
    /// Partially based on: https://rogerjohansson.blog/2008/02/28/linq-expressions-creating-objects/
    /// </summary>
    public static class Activator
    {
        public delegate object ExpressionActivatorDelegate(params object[] args);

        private const BindingFlags InternalBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly Lazy<ConcurrentDictionary<Type, ExpressionActivatorDelegate>> Cache =
            new Lazy<ConcurrentDictionary<Type, ExpressionActivatorDelegate>>(() =>
                new ConcurrentDictionary<Type, ExpressionActivatorDelegate>());

        /// <summary>
        /// Creates an object instance of the passed type. Has support for
        /// constructor arguments.
        /// </summary>
        /// <typeparam name="TConstruct">The type which should be instantiated.</typeparam>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>A object instance of the passed type.</returns>
        public static TConstruct CreateInstance<TConstruct>(
            params object[] args)

            where TConstruct : class
        {
            return CreateInstance(typeof(TConstruct), args) as TConstruct;
        }

        /// <summary>
        /// Creates an object instance of the passed type. Has support for
        /// constructor arguments.
        /// </summary>
        /// <param name="type">The type which should be instantiated.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <returns>A object instance of the passed type.</returns>
        public static object CreateInstance(
            Type type,
            params object[] args)
        {
            Argument.NotNull(
                type,
                nameof(type));

            if (Cache.Value.TryGetValue(type, out var activator))
                return activator.Invoke(args);

            activator = GetActivator(type, args);
            Cache.Value.TryAdd(type, activator);

            return activator.Invoke(args);
        }

        private static ExpressionActivatorDelegate GetActivator(
            Type type,
            params object[] args)
        {
            var ctor = GetTypeConstructor(
                type,
                args);

            if (ctor == null)
                return null;

            var parameters = ctor.GetParameters();

            var param = Expression.Parameter(
                typeof(object[]),
                "args");

            var argsExp = new Expression[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = parameters[i].ParameterType;

                var paramAccessorExp = Expression.ArrayIndex(
                    param,
                    index);

                var paramCastExp = Expression.Convert(
                    paramAccessorExp,
                    paramType);

                argsExp[i] = paramCastExp;
            }

            var newExp = Expression.New(
                ctor,
                argsExp);

            var lambda = Expression.Lambda(
                typeof(ExpressionActivatorDelegate),
                newExp,
                param);

            return lambda.Compile() as ExpressionActivatorDelegate;
        }

        private static ConstructorInfo GetTypeConstructor(
            Type type,
            params object[] args)
        {
            if (!args.Any())
                return GetParameterlessConstructor(
                    type);

            return GetMatchingTypeArgumentConstructor(
                type,
                args);
        }

        private static ConstructorInfo GetParameterlessConstructor(
            Type type)
        {
            if (type == null)
                return null;

            var ctor = type.GetConstructor(Type.EmptyTypes) ??
                       type.GetConstructor(InternalBindingFlags, null, Type.EmptyTypes,
                           null);

            return ctor;
        }

        private static ConstructorInfo GetMatchingTypeArgumentConstructor(
            Type type,
            object[] args)
        {
            if (type == null || args == null || !args.Any())
                return null;

            var typeArray = Type.GetTypeArray(args);

            var ctor = type.GetConstructor(typeArray) ??
                       type.GetConstructor(InternalBindingFlags, null, typeArray, null);

            return ctor;
        }
    }
}