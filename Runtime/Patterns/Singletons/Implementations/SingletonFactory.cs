using System;
using System.Linq;
using System.Reflection;

namespace EasyToolKit.Core.Patterns.Implementations
{
    /// <summary>
    /// Factory for creating standard C# singleton instances.
    /// </summary>
    internal static class SingletonFactory
    {
        /// <summary>
        /// Creates a singleton instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>The created singleton instance.</returns>
        /// <exception cref="SingletonInitializationException">Thrown if creation fails.</exception>
        public static T Create<T>() where T : class, ILifecycleSingleton
        {
            var type = typeof(T);
            var constructor = FindValidConstructor(type);
            var instance = constructor.Invoke(null) as T;

            if (instance == null)
                throw new SingletonInitializationException(
                    $"[Singleton] ConstructionFailed: Failed to create instance of type '{type.Name}'. " +
                    $"Ensure the constructor properly initializes the base class.",
                    type);

            return instance;
        }

        /// <summary>
        /// Finds a valid constructor for the singleton type.
        /// </summary>
        /// <param name="type">The type to search.</param>
        /// <returns>A valid constructor.</returns>
        /// <exception cref="SingletonInitializationException">Thrown if no valid constructor exists.</exception>
        private static ConstructorInfo FindValidConstructor(Type type)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var constructors = type.GetConstructors(bindingFlags);

            // Prefer parameterless non-public constructor
            var ctor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);

            if (ctor == null || ctor.IsPublic)
                throw new SingletonInitializationException(
                    $"[Singleton] InvalidConstructor: Type '{type.Name}' must have a non-public " +
                    $"parameterless constructor. Add 'private {type.Name}() {{ }}' or 'protected {type.Name}() {{ }}'.",
                    type);

            return ctor;
        }
    }
}
