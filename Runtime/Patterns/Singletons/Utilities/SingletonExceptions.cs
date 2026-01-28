using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Exception thrown when singleton initialization fails.
    /// </summary>
    public class SingletonInitializationException : InvalidOperationException
    {
        /// <summary>
        /// Gets the type of singleton that failed to initialize.
        /// </summary>
        public Type SingletonType { get; }

        /// <summary>
        /// Initializes a new instance with a message and type.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="type">The singleton type.</param>
        public SingletonInitializationException(string message, Type type)
            : base(message)
        {
            SingletonType = type;
        }

        /// <summary>
        /// Initializes a new instance with a message, type, and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="type">The singleton type.</param>
        /// <param name="innerException">The inner exception.</param>
        public SingletonInitializationException(
            string message,
            Type type,
            Exception innerException)
            : base(message, innerException)
        {
            SingletonType = type;
        }
    }

    /// <summary>
    /// Exception thrown when accessing a destroyed singleton.
    /// </summary>
    public class SingletonDestroyedException : InvalidOperationException
    {
        /// <summary>
        /// Gets the type of singleton that was destroyed.
        /// </summary>
        public Type SingletonType { get; }

        /// <summary>
        /// Initializes a new instance with a message and type.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="type">The singleton type.</param>
        public SingletonDestroyedException(string message, Type type)
            : base(message)
        {
            SingletonType = type;
        }
    }

    /// <summary>
    /// Exception thrown when a required singleton is not found.
    /// </summary>
    public class SingletonNotFoundException : InvalidOperationException
    {
        /// <summary>
        /// Gets the type of singleton that was not found.
        /// </summary>
        public Type SingletonType { get; }

        /// <summary>
        /// Initializes a new instance with a message and type.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="type">The singleton type.</param>
        public SingletonNotFoundException(string message, Type type)
            : base(message)
        {
            SingletonType = type;
        }
    }
}
