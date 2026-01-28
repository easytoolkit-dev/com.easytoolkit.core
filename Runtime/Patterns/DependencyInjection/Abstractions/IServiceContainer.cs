using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Represents a dependency injection container.
    /// </summary>
    public interface IServiceContainer : IServiceProvider, IDisposable
    {
        /// <summary>
        /// Creates a new service scope.
        /// </summary>
        IServiceScope CreateScope();
    }
}
