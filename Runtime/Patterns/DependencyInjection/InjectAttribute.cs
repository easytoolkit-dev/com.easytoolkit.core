using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Marks an instance field as a required dependency injection target.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class InjectAttribute : Attribute
    {
    }
}
