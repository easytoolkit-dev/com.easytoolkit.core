using System;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Provides utility methods for creating accessor and invoker delegates from reflection metadata.
    /// </summary>
    /// <remarks>
    /// This utility class provides methods to create high-performance delegates for accessing fields,
    /// properties, and invoking methods using reflection. All delegates use weak types (object) for
    /// maximum flexibility. Future versions may include expression tree compilation for improved performance.
    /// </remarks>
    public static class ReflectionUtility
    {
        #region Field Accessors

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

            return () => fieldInfo.GetValue(null);
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

            return (ref object instance) => fieldInfo.GetValue(instance);
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

            return (object value) => fieldInfo.SetValue(null, value);
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

            return (ref object instance, object value) => fieldInfo.SetValue(instance, value);
        }

        #endregion

        #region Property Accessors

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
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a getter.", nameof(propertyInfo));
            }

            if (!getMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not static.", nameof(propertyInfo));
            }

            return () => propertyInfo.GetValue(null);
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
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a getter.", nameof(propertyInfo));
            }

            if (getMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not an instance property.", nameof(propertyInfo));
            }

            return (ref object instance) => propertyInfo.GetValue(instance);
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
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a setter.", nameof(propertyInfo));
            }

            if (!setMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not static.", nameof(propertyInfo));
            }

            return (object value) => propertyInfo.SetValue(null, value);
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
                throw new ArgumentException($"Property '{propertyInfo.Name}' does not have a setter.", nameof(propertyInfo));
            }

            if (setMethod.IsStatic)
            {
                throw new ArgumentException($"Property '{propertyInfo.Name}' is not an instance property.", nameof(propertyInfo));
            }

            return (ref object instance, object value) => propertyInfo.SetValue(instance, value);
        }

        #endregion

        #region Method Invokers

        /// <summary>
        /// Creates an invoker delegate for calling the specified static method.
        /// </summary>
        /// <param name="methodInfo">The static method metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the static method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not a static method.</exception>
        public static StaticFuncInvoker CreateStaticMethodInvoker(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!methodInfo.IsStatic)
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' is not static.", nameof(methodInfo));
            }

            return args => methodInfo.Invoke(null, args);
        }

        /// <summary>
        /// Creates an invoker delegate for calling the specified instance method.
        /// </summary>
        /// <param name="methodInfo">The instance method metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the instance method when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="methodInfo"/> is not an instance method (i.e., it is a static method).</exception>
        public static InstanceFuncInvoker CreateInstanceMethodInvoker(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (methodInfo.IsStatic)
            {
                throw new ArgumentException($"Method '{methodInfo.Name}' is not an instance method.", nameof(methodInfo));
            }

            return (ref object instance, object[] args) => methodInfo.Invoke(instance, args);
        }

        #endregion

        #region Constructor Invokers

        /// <summary>
        /// Creates an invoker delegate for calling the specified constructor.
        /// </summary>
        /// <param name="constructorInfo">The constructor metadata to create an invoker for.</param>
        /// <returns>A delegate that invokes the constructor when called.</returns>
        public static ConstructorInvoker CreateConstructorInvoker(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            return args => constructorInfo.Invoke(args);
        }

        /// <summary>
        /// Creates a strongly-typed invoker delegate for calling the specified parameterless constructor.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return value (the constructed instance type).</typeparam>
        /// <param name="constructorInfo">The constructor metadata to create an invoker for.</param>
        /// <returns>A strongly-typed delegate that invokes the parameterless constructor when called.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="constructorInfo"/> has parameters.</exception>
        public static ConstructorInvoker<TReturn> CreateConstructorInvoker<TReturn>(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var parameters = constructorInfo.GetParameters();
            if (parameters.Length != 0)
            {
                throw new ArgumentException(
                    $"Constructor must be parameterless, but has {parameters.Length} parameter(s).",
                    nameof(constructorInfo));
            }

            return () => (TReturn)constructorInfo.Invoke(null);
        }

        #endregion
    }
}
