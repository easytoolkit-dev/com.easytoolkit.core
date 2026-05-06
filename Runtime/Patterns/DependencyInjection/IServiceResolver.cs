using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Defines a mechanism for retrieving service objects.
    /// </summary>
    public interface IServiceResolver
    {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        object GetService(Type serviceType);
    }
}
