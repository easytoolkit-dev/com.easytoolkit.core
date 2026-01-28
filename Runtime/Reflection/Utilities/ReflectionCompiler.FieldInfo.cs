#if UNITY_EDITOR || !ENABLE_IL2CPP
#define ENABLE_COMPILER
#endif

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
{
    public static partial class ReflectionCompiler
    {
        /// <summary>
        /// Creates a getter delegate for accessing the specified static field value.
        /// </summary>
        /// <param name="fieldInfo">The static field metadata to create a getter for.</param>
        /// <returns>A delegate that retrieves the static field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not a static field.</exception>
        public static StaticGetter CreateStaticFieldGetter(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            if (!fieldInfo.IsStatic)
            {
                throw new ArgumentException($"Field '{fieldInfo.Name}' is not static.", nameof(fieldInfo));
            }

#if ENABLE_COMPILER
            // Create an expression to access the static field
            var fieldExpression = Expression.Field(null, fieldInfo);

            // Convert to object if the field type is not already object
            var bodyExpression = fieldInfo.FieldType == typeof(object)
                ? (Expression)fieldExpression
                : Expression.Convert(fieldExpression, typeof(object));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticGetter>(bodyExpression);
            return lambda.Compile();
#else
            return () => fieldInfo.GetValue(null);
#endif
        }

        /// <summary>
        /// Creates a getter delegate for accessing the specified instance field value.
        /// </summary>
        /// <param name="fieldInfo">The instance field metadata to create a getter for.</param>
        /// <returns>A delegate that retrieves the instance field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not an instance field (i.e., it is a static field).</exception>
        public static InstanceGetter CreateInstanceFieldGetter(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            if (fieldInfo.IsStatic)
            {
                throw new ArgumentException($"Field '{fieldInfo.Name}' is not an instance field.", nameof(fieldInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expression for the instance
            var instanceParameter = Expression.Parameter(typeof(object), "instance");

            // Convert the instance parameter to the declaring type
            var convertedInstance = Expression.Convert(instanceParameter, fieldInfo.DeclaringType);

            // Create an expression to access the instance field
            var fieldExpression = Expression.Field(convertedInstance, fieldInfo);

            // Convert to object if the field type is not already object
            var bodyExpression = fieldInfo.FieldType == typeof(object)
                ? (Expression)fieldExpression
                : Expression.Convert(fieldExpression, typeof(object));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceGetter>(bodyExpression, instanceParameter);
            return lambda.Compile();
#else
            return fieldInfo.GetValue;
#endif
        }

        /// <summary>
        /// Creates a setter delegate for modifying the specified static field value.
        /// </summary>
        /// <param name="fieldInfo">The static field metadata to create a setter for.</param>
        /// <returns>A delegate that sets the static field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not a static field.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is read-only (init-only).</exception>
        public static StaticSetter CreateStaticFieldSetter(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            if (!fieldInfo.IsStatic)
            {
                throw new ArgumentException($"Field '{fieldInfo.Name}' is not static.", nameof(fieldInfo));
            }

            if (fieldInfo.IsInitOnly)
            {
                throw new ArgumentException($"Field '{fieldInfo.Name}' is read-only.", nameof(fieldInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expression for the value
            var valueParameter = Expression.Parameter(typeof(object), "value");

            // Create an expression to access the static field
            var fieldExpression = Expression.Field(null, fieldInfo);

            // Convert the value parameter to the field type
            var convertedValue = Expression.Convert(valueParameter, fieldInfo.FieldType);

            // Create assignment expression
            var assignExpression = Expression.Assign(fieldExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticSetter>(assignExpression, valueParameter);
            return lambda.Compile();
#else
            return value => fieldInfo.SetValue(null, value);
#endif
        }

        /// <summary>
        /// Creates a setter delegate for modifying the specified instance field value.
        /// </summary>
        /// <param name="fieldInfo">The instance field metadata to create a setter for.</param>
        /// <returns>A delegate that sets the instance field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not an instance field (i.e., it is a static field).</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is read-only (init-only).</exception>
        public static InstanceSetter CreateInstanceFieldSetter(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            if (fieldInfo.IsStatic)
            {
                throw new ArgumentException($"Field '{fieldInfo.Name}' is not an instance field.", nameof(fieldInfo));
            }

            if (fieldInfo.IsInitOnly)
            {
                throw new ArgumentException($"Field '{fieldInfo.Name}' is read-only.", nameof(fieldInfo));
            }

#if ENABLE_COMPILER
            // Create parameter expressions
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var valueParameter = Expression.Parameter(typeof(object), "value");

            // Convert the instance parameter to the declaring type
            var convertedInstance = Expression.Convert(instanceParameter, fieldInfo.DeclaringType);

            // Create an expression to access the instance field
            var fieldExpression = Expression.Field(convertedInstance, fieldInfo);

            // Convert the value parameter to the field type
            var convertedValue = Expression.Convert(valueParameter, fieldInfo.FieldType);

            // Create assignment expression
            var assignExpression = Expression.Assign(fieldExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceSetter>(assignExpression, instanceParameter, valueParameter);
            return lambda.Compile();
#else
            return fieldInfo.SetValue;
#endif
        }
    }
}
