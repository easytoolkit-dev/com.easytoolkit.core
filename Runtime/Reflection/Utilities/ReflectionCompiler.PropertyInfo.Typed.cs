#if UNITY_EDITOR || !ENABLE_IL2CPP
#define ENABLE_COMPILER
#endif

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
{
    public static partial class ReflectionCompiler
    {
        /// <summary>
        /// Creates a strongly-typed getter delegate for accessing the specified static property value.
        /// </summary>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="propertyInfo">The static property metadata to create a getter for.</param>
        /// <returns>A strongly-typed delegate that retrieves the static property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not a static property.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a getter.</exception>
        public static StaticGetter<TValue> CreateStaticPropertyGetter<TValue>(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var getMethod = propertyInfo.GetMethod;
            if (getMethod == null || !getMethod.IsPublic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a getter.",
                    nameof(propertyInfo));
            }

            if (!getMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not static.", nameof(propertyInfo));
            }

#if ENABLE_COMPILER
            // Create an expression to access the static property
            var propertyExpression = Expression.Property(null, propertyInfo);

            // Convert to TValue if the property type is not already TValue
            var bodyExpression = propertyInfo.PropertyType == typeof(TValue)
                ? (Expression)propertyExpression
                : Expression.Convert(propertyExpression, typeof(TValue));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticGetter<TValue>>(bodyExpression);
            return lambda.Compile();
#else
            return () => (TValue)propertyInfo.GetValue(null);
#endif
        }

        /// <summary>
        /// Creates a strongly-typed getter delegate for accessing the specified instance property value.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="propertyInfo">The instance property metadata to create a getter for.</param>
        /// <returns>A strongly-typed delegate that retrieves the instance property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not an instance property (i.e., it is a static property).</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a getter.</exception>
        public static InstanceGetter<TInstance, TValue> CreateInstancePropertyGetter<TInstance, TValue>(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var getMethod = propertyInfo.GetMethod;
            if (getMethod == null || !getMethod.IsPublic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a getter.",
                    nameof(propertyInfo));
            }

            if (getMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not an instance property.",
                    nameof(propertyInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expression for the instance
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");

            // Create an expression to access the instance property
            var propertyExpression = Expression.Property(instanceParameter, propertyInfo);

            // Convert to TValue if the property type is not already TValue
            var bodyExpression = propertyInfo.PropertyType == typeof(TValue)
                ? (Expression)propertyExpression
                : Expression.Convert(propertyExpression, typeof(TValue));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceGetter<TInstance, TValue>>(bodyExpression, instanceParameter);
            return lambda.Compile();
#else
            return (ref TInstance instance) => (TValue)propertyInfo.GetValue(instance);
#endif
        }

        /// <summary>
        /// Creates a strongly-typed setter delegate for modifying the specified static property value.
        /// </summary>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="propertyInfo">The static property metadata to create a setter for.</param>
        /// <returns>A strongly-typed delegate that sets the static property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not a static property.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a setter.</exception>
        public static StaticSetter<TValue> CreateStaticPropertySetter<TValue>(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var setMethod = propertyInfo.SetMethod;
            if (setMethod == null || !setMethod.IsPublic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a setter.",
                    nameof(propertyInfo));
            }

            if (!setMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not static.", nameof(propertyInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expression for the value
            var valueParameter = Expression.Parameter(typeof(TValue), "value");

            // Create an expression to access the static property
            var propertyExpression = Expression.Property(null, propertyInfo);

            // Convert the value parameter to the property type
            var convertedValue = propertyInfo.PropertyType == typeof(TValue)
                ? (Expression)valueParameter
                : Expression.Convert(valueParameter, propertyInfo.PropertyType);

            // Create assignment expression
            var assignExpression = Expression.Assign(propertyExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticSetter<TValue>>(assignExpression, valueParameter);
            return lambda.Compile();
#else
            return (TValue value) => propertyInfo.SetValue(null, value);
#endif
        }

        /// <summary>
        /// Creates a strongly-typed setter delegate for modifying the specified instance property value.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="propertyInfo">The instance property metadata to create a setter for.</param>
        /// <returns>A strongly-typed delegate that sets the instance property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not an instance property (i.e., it is a static property).</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a setter.</exception>
        public static InstanceSetter<TInstance, TValue> CreateInstancePropertySetter<TInstance, TValue>(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var setMethod = propertyInfo.SetMethod;
            if (setMethod == null || !setMethod.IsPublic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a setter.",
                    nameof(propertyInfo));
            }

            if (setMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not an instance property.",
                    nameof(propertyInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expressions
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var valueParameter = Expression.Parameter(typeof(TValue), "value");

            // Create an expression to access the instance property
            var propertyExpression = Expression.Property(instanceParameter, propertyInfo);

            // Convert the value parameter to the property type
            var convertedValue = propertyInfo.PropertyType == typeof(TValue)
                ? (Expression)valueParameter
                : Expression.Convert(valueParameter, propertyInfo.PropertyType);

            // Create assignment expression
            var assignExpression = Expression.Assign(propertyExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceSetter<TInstance, TValue>>(assignExpression, instanceParameter, valueParameter);
            return lambda.Compile();
#else
            return (ref TInstance instance, TValue value) => propertyInfo.SetValue(instance, value);
#endif
        }
    }
}
