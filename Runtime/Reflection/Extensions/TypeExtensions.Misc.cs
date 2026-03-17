using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Provides extension methods for System.Type reflection operations.
    /// </summary>
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Tries to create an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="instance">When this method returns, contains the created instance, or null if creation failed.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>true if an instance was created; otherwise, false.</returns>
        public static bool TryCreateInstance(this Type type, out object instance, params object[] args)
        {
            instance = null;
            try
            {
                instance = type.CreateInstance(args);
                return instance != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to create an instance of the specified type as type T.
        /// </summary>
        /// <typeparam name="T">The type to cast the instance to.</typeparam>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="instance">When this method returns, contains the created instance, or default if creation failed.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>true if an instance was created; otherwise, false.</returns>
        public static bool TryCreateInstance<T>(this Type type, out T instance, params object[] args)
        {
            instance = default;
            try
            {
                instance = type.CreateInstance<T>(args);
                return instance != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>A new instance of the specified type.</returns>
        public static object CreateInstance(this Type type, params object[] args)
        {
            if (type == null)
                return null;

            if (type == typeof(string))
                return string.Empty;

            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Creates an instance of the specified type as type T.
        /// </summary>
        /// <typeparam name="T">The type to cast the instance to.</typeparam>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>A new instance of the specified type cast to T.</returns>
        /// <exception cref="ArgumentException">Generic type T is not assignable from the created instance.</exception>
        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            if (!typeof(T).IsAssignableFrom(type))
                throw new ArgumentException($"Generic type '{typeof(T)}' must be convertible by '{type}'");
            return (T)CreateInstance(type, args);
        }
    }
}
