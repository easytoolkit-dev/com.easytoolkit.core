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
        /// Creates a getter delegate for accessing the specified static property value.
        /// </summary>
        /// <param name="propertyInfo">The static property metadata to create a getter for.</param>
        /// <returns>A delegate that retrieves the static property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not a static property.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a getter.</exception>
        public static StaticGetter CreateStaticPropertyGetter(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var getMethod = propertyInfo.GetMethod;
            if (getMethod == null)
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

            // Convert to object if the property type is not already object
            var bodyExpression = propertyInfo.PropertyType == typeof(object)
                ? (Expression)propertyExpression
                : Expression.Convert(propertyExpression, typeof(object));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticGetter>(bodyExpression);
            return lambda.Compile();
#else
            return () => propertyInfo.GetValue(null);
#endif
        }

        /// <summary>
        /// Creates a getter delegate for accessing the specified instance property value.
        /// </summary>
        /// <param name="propertyInfo">The instance property metadata to create a getter for.</param>
        /// <returns>A delegate that retrieves the instance property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not an instance property (i.e., it is a static property).</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a getter.</exception>
        public static InstanceGetter CreateInstancePropertyGetter(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var getMethod = propertyInfo.GetMethod;
            if (getMethod == null)
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
            var instanceParameter = Expression.Parameter(typeof(object), "instance");

            // Convert the instance parameter to the declaring type
            var convertedInstance = Expression.Convert(instanceParameter, propertyInfo.DeclaringType);

            // Create an expression to access the instance property
            var propertyExpression = Expression.Property(convertedInstance, propertyInfo);

            // Convert to object if the property type is not already object
            var bodyExpression = propertyInfo.PropertyType == typeof(object)
                ? (Expression)propertyExpression
                : Expression.Convert(propertyExpression, typeof(object));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceGetter>(bodyExpression, instanceParameter);
            return lambda.Compile();
#else
            return propertyInfo.GetValue;
#endif
        }

        /// <summary>
        /// Creates a setter delegate for modifying the specified static property value.
        /// </summary>
        /// <param name="propertyInfo">The static property metadata to create a setter for.</param>
        /// <returns>A delegate that sets the static property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not a static property.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a setter.</exception>
        public static StaticSetter CreateStaticPropertySetter(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var setMethod = propertyInfo.SetMethod;
            if (setMethod == null)
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
            var valueParameter = Expression.Parameter(typeof(object), "value");

            // Create an expression to access the static property
            var propertyExpression = Expression.Property(null, propertyInfo);

            // Convert the value parameter to the property type
            var convertedValue = Expression.Convert(valueParameter, propertyInfo.PropertyType);

            // Create assignment expression
            var assignExpression = Expression.Assign(propertyExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticSetter>(assignExpression, valueParameter);
            return lambda.Compile();
#else
            return (object value) => propertyInfo.SetValue(null, value);
#endif
        }

        /// <summary>
        /// Creates a setter delegate for modifying the specified instance property value.
        /// </summary>
        /// <param name="propertyInfo">The instance property metadata to create a setter for.</param>
        /// <returns>A delegate that sets the instance property value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> is not an instance property (i.e., it is a static property).</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyInfo"/> does not have a setter.</exception>
        public static InstanceSetter CreateInstancePropertySetter(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var setMethod = propertyInfo.SetMethod;
            if (setMethod == null)
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
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var valueParameter = Expression.Parameter(typeof(object), "value");

            // Convert the instance parameter to the declaring type
            var convertedInstance = Expression.Convert(instanceParameter, propertyInfo.DeclaringType);

            // Create an expression to access the instance property
            var propertyExpression = Expression.Property(convertedInstance, propertyInfo);

            // Convert the value parameter to the property type
            var convertedValue = Expression.Convert(valueParameter, propertyInfo.PropertyType);

            // Create assignment expression
            var assignExpression = Expression.Assign(propertyExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceSetter>(assignExpression, instanceParameter, valueParameter);
            return lambda.Compile();
#else
            return propertyInfo.SetValue;
#endif
        }
    }
}
