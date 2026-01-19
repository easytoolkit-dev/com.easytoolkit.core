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
        /// Creates a strongly-typed getter delegate for accessing the specified static field value.
        /// </summary>
        /// <typeparam name="TValue">The type of the field value.</typeparam>
        /// <param name="fieldInfo">The static field metadata to create a getter for.</param>
        /// <returns>A strongly-typed delegate that retrieves the static field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not a static field.</exception>
        public static StaticGetter<TValue> CreateStaticFieldGetter<TValue>(FieldInfo fieldInfo)
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

            // Convert to TValue if the field type is not already TValue
            var bodyExpression = fieldInfo.FieldType == typeof(TValue)
                ? (Expression)fieldExpression
                : Expression.Convert(fieldExpression, typeof(TValue));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticGetter<TValue>>(bodyExpression);
            return lambda.Compile();
#else
            return () => (TValue)fieldInfo.GetValue(null);
#endif
        }

        /// <summary>
        /// Creates a strongly-typed getter delegate for accessing the specified instance field value.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TValue">The type of the field value.</typeparam>
        /// <param name="fieldInfo">The instance field metadata to create a getter for.</param>
        /// <returns>A strongly-typed delegate that retrieves the instance field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not an instance field (i.e., it is a static field).</exception>
        public static InstanceGetter<TInstance, TValue> CreateInstanceFieldGetter<TInstance, TValue>(FieldInfo fieldInfo)
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
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");

            // Create an expression to access the instance field
            var fieldExpression = Expression.Field(instanceParameter, fieldInfo);

            // Convert to TValue if the field type is not already TValue
            var bodyExpression = fieldInfo.FieldType == typeof(TValue)
                ? (Expression)fieldExpression
                : Expression.Convert(fieldExpression, typeof(TValue));

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceGetter<TInstance, TValue>>(bodyExpression, instanceParameter);
            return lambda.Compile();
#else
            return (ref TInstance instance) => (TValue)fieldInfo.GetValue(instance);
#endif
        }

        /// <summary>
        /// Creates a strongly-typed setter delegate for modifying the specified static field value.
        /// </summary>
        /// <typeparam name="TValue">The type of the field value.</typeparam>
        /// <param name="fieldInfo">The static field metadata to create a setter for.</param>
        /// <returns>A strongly-typed delegate that sets the static field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not a static field.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is read-only (init-only).</exception>
        public static StaticSetter<TValue> CreateStaticFieldSetter<TValue>(FieldInfo fieldInfo)
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
            var valueParameter = Expression.Parameter(typeof(TValue), "value");

            // Create an expression to access the static field
            var fieldExpression = Expression.Field(null, fieldInfo);

            // Convert the value parameter to the field type
            var convertedValue = fieldInfo.FieldType == typeof(TValue)
                ? (Expression)valueParameter
                : Expression.Convert(valueParameter, fieldInfo.FieldType);

            // Create assignment expression
            var assignExpression = Expression.Assign(fieldExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<StaticSetter<TValue>>(assignExpression, valueParameter);
            return lambda.Compile();
#else
            return (TValue value) => fieldInfo.SetValue(null, value);
#endif
        }

        /// <summary>
        /// Creates a strongly-typed setter delegate for modifying the specified instance field value.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TValue">The type of the field value.</typeparam>
        /// <param name="fieldInfo">The instance field metadata to create a setter for.</param>
        /// <returns>A strongly-typed delegate that sets the instance field value when invoked.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is not an instance field (i.e., it is a static field).</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldInfo"/> is read-only (init-only).</exception>
        public static InstanceSetter<TInstance, TValue> CreateInstanceFieldSetter<TInstance, TValue>(FieldInfo fieldInfo)
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
            var instanceParameter = Expression.Parameter(typeof(TInstance).MakeByRefType(), "instance");
            var valueParameter = Expression.Parameter(typeof(TValue), "value");

            // Create an expression to access the instance field
            var fieldExpression = Expression.Field(instanceParameter, fieldInfo);

            // Convert the value parameter to the field type
            var convertedValue = fieldInfo.FieldType == typeof(TValue)
                ? (Expression)valueParameter
                : Expression.Convert(valueParameter, fieldInfo.FieldType);

            // Create assignment expression
            var assignExpression = Expression.Assign(fieldExpression, convertedValue);

            // Create and compile the lambda expression
            var lambda = Expression.Lambda<InstanceSetter<TInstance, TValue>>(assignExpression, instanceParameter, valueParameter);
            return lambda.Compile();
#else
            return (ref TInstance instance, TValue value) => fieldInfo.SetValue(instance, value);
#endif
        }
    }
}
