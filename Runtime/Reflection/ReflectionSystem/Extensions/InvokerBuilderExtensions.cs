using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Extension methods for <see cref="IInvokerBuilder"/> providing generic API support.
    /// </summary>
    public static class InvokerBuilderExtensions
    {
        #region Static Methods - 0 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking a parameterless static method.
        /// </summary>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the static method.</param>
        /// <returns>A typed delegate that invokes the static method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static StaticFuncInvoker<TResult> BuildStaticFunc<TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticFuncInvoker invoker = builder.BuildStaticFunc(targetType, Type.EmptyTypes);
            return () => (TResult)invoker();
        }

        #endregion

        #region Static Methods - 1 Parameter

        /// <summary>
        /// Builds a typed delegate for invoking a static method with one parameter.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the static method.</param>
        /// <returns>A typed delegate that invokes the static method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static StaticFuncInvoker<T, TResult> BuildStaticFunc<T, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticFuncInvoker invoker = builder.BuildStaticFunc(targetType, typeof(T));
            return arg => (TResult)invoker(arg);
        }

        #endregion

        #region Static Methods - 2 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking a static method with two parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the static method.</param>
        /// <returns>A typed delegate that invokes the static method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static StaticFuncInvoker<T1, T2, TResult> BuildStaticFunc<T1, T2, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticFuncInvoker invoker = builder.BuildStaticFunc(targetType, typeof(T1), typeof(T2));
            return (arg1, arg2) => (TResult)invoker(arg1, arg2);
        }

        #endregion

        #region Static Methods - 3 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking a static method with three parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the static method.</param>
        /// <returns>A typed delegate that invokes the static method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static StaticFuncInvoker<T1, T2, T3, TResult> BuildStaticFunc<T1, T2, T3, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticFuncInvoker invoker = builder.BuildStaticFunc(targetType, typeof(T1), typeof(T2), typeof(T3));
            return (arg1, arg2, arg3) => (TResult)invoker(arg1, arg2, arg3);
        }

        #endregion

        #region Static Methods - 4 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking a static method with four parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the static method.</param>
        /// <returns>A typed delegate that invokes the static method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static StaticFuncInvoker<T1, T2, T3, T4, TResult> BuildStaticFunc<T1, T2, T3, T4, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticFuncInvoker invoker = builder.BuildStaticFunc(targetType, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            return (arg1, arg2, arg3, arg4) => (TResult)invoker(arg1, arg2, arg3, arg4);
        }

        #endregion

        #region Static Methods - 5 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking a static method with five parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the static method.</param>
        /// <returns>A typed delegate that invokes the static method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static StaticFuncInvoker<T1, T2, T3, T4, T5, TResult> BuildStaticFunc<T1, T2, T3, T4, T5, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticFuncInvoker invoker = builder.BuildStaticFunc(targetType, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            return (arg1, arg2, arg3, arg4, arg5) => (TResult)invoker(arg1, arg2, arg3, arg4, arg5);
        }

        #endregion

        #region Instance Methods - 0 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking a parameterless instance method.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static TypedInstanceFuncInvoker<TInstance, TResult> BuildTypedInstanceFunc<TInstance, TResult>(this IInvokerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(typeof(TInstance), Type.EmptyTypes);
            return (ref TInstance instance) =>
            {
                object boxedInstance = instance;
                var result = invoker(ref boxedInstance);
                instance = (TInstance)boxedInstance;
                return (TResult)result;
            };
        }

        #endregion

        #region Instance Methods - 1 Parameter

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with one parameter.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="T">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static TypedInstanceFuncInvoker<TInstance, T, TResult> BuildTypedInstanceFunc<TInstance, T, TResult>(this IInvokerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(typeof(TInstance), typeof(T));
            return (ref TInstance instance, T arg) =>
            {
                object boxedInstance = instance;
                var result = invoker(ref boxedInstance, arg);
                instance = (TInstance)boxedInstance;
                return (TResult)result;
            };
        }

        #endregion

        #region Instance Methods - 2 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with two parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static TypedInstanceFuncInvoker<TInstance, T1, T2, TResult> BuildTypedInstanceFunc<TInstance, T1, T2, TResult>(this IInvokerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(typeof(TInstance), typeof(T1), typeof(T2));
            return (ref TInstance instance, T1 arg1, T2 arg2) =>
            {
                object boxedInstance = instance;
                var result = invoker(ref boxedInstance, arg1, arg2);
                instance = (TInstance)boxedInstance;
                return (TResult)result;
            };
        }

        #endregion

        #region Instance Methods - 3 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with three parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static TypedInstanceFuncInvoker<TInstance, T1, T2, T3, TResult> BuildTypedInstanceFunc<TInstance, T1, T2, T3, TResult>(this IInvokerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(typeof(TInstance), typeof(T1), typeof(T2), typeof(T3));
            return (ref TInstance instance, T1 arg1, T2 arg2, T3 arg3) =>
            {
                object boxedInstance = instance;
                var result = invoker(ref boxedInstance, arg1, arg2, arg3);
                instance = (TInstance)boxedInstance;
                return (TResult)result;
            };
        }

        #endregion

        #region Instance Methods - 4 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with four parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static TypedInstanceFuncInvoker<TInstance, T1, T2, T3, T4, TResult> BuildTypedInstanceFunc<TInstance, T1, T2, T3, T4, TResult>(this IInvokerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(typeof(TInstance), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            return (ref TInstance instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
            {
                object boxedInstance = instance;
                var result = invoker(ref boxedInstance, arg1, arg2, arg3, arg4);
                instance = (TInstance)boxedInstance;
                return (TResult)result;
            };
        }

        #endregion

        #region Instance Methods - 5 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with five parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static TypedInstanceFuncInvoker<TInstance, T1, T2, T3, T4, T5, TResult> BuildTypedInstanceFunc<TInstance, T1, T2, T3, T4, T5, TResult>(this IInvokerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(typeof(TInstance), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            return (ref TInstance instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
            {
                object boxedInstance = instance;
                var result = invoker(ref boxedInstance, arg1, arg2, arg3, arg4, arg5);
                instance = (TInstance)boxedInstance;
                return (TResult)result;
            };
        }

        #endregion

        #region Instance Methods with Object Instance - 0 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking a parameterless instance method with object instance.
        /// </summary>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static InstanceFuncInvoker<TResult> BuildInstanceFunc<TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(targetType, Type.EmptyTypes);
            return (ref object instance) => (TResult)invoker(ref instance);
        }

        #endregion

        #region Instance Methods with Object Instance - 1 Parameter

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with one parameter and object instance.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static InstanceFuncInvoker<T, TResult> BuildInstanceFunc<T, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(targetType, typeof(T));
            return (ref object instance, T arg) => (TResult)invoker(ref instance, arg);
        }

        #endregion

        #region Instance Methods with Object Instance - 2 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with two parameters and object instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static InstanceFuncInvoker<T1, T2, TResult> BuildInstanceFunc<T1, T2, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(targetType, typeof(T1), typeof(T2));
            return (ref object instance, T1 arg1, T2 arg2) => (TResult)invoker(ref instance, arg1, arg2);
        }

        #endregion

        #region Instance Methods with Object Instance - 3 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with three parameters and object instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static InstanceFuncInvoker<T1, T2, T3, TResult> BuildInstanceFunc<T1, T2, T3, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(targetType, typeof(T1), typeof(T2), typeof(T3));
            return (ref object instance, T1 arg1, T2 arg2, T3 arg3) => (TResult)invoker(ref instance, arg1, arg2, arg3);
        }

        #endregion

        #region Instance Methods with Object Instance - 4 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with four parameters and object instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static InstanceFuncInvoker<T1, T2, T3, T4, TResult> BuildInstanceFunc<T1, T2, T3, T4, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(targetType, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            return (ref object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => (TResult)invoker(ref instance, arg1, arg2, arg3, arg4);
        }

        #endregion

        #region Instance Methods with Object Instance - 5 Parameters

        /// <summary>
        /// Builds a typed delegate for invoking an instance method with five parameters and object instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the method.</typeparam>
        /// <param name="builder">The invoker builder.</param>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <returns>A typed delegate that invokes the instance method.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public static InstanceFuncInvoker<T1, T2, T3, T4, T5, TResult> BuildInstanceFunc<T1, T2, T3, T4, T5, TResult>(this IInvokerBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceFuncInvoker invoker = builder.BuildInstanceFunc(targetType, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            return (ref object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => (TResult)invoker(ref instance, arg1, arg2, arg3, arg4, arg5);
        }

        #endregion
    }
}
