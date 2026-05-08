using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Performs field injection for services that use <see cref="InjectAttribute"/>.
    /// </summary>
    internal static class ServiceInjector
    {
        private static readonly Dictionary<Type, FieldInfo[]> FieldsByType = new();
        private static readonly object SyncLock = new();

        /// <summary>
        /// Injects marked fields on the specified target.
        /// </summary>
        public static void Inject(IServiceProvider provider, object target)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var targetType = target.GetType();
            var fields = GetInjectableFields(targetType);

            foreach (var field in fields)
            {
                var value = provider.GetService(field.FieldType);
                if (value == null)
                {
                    throw new DependencyResolutionException(
                        $"Unable to resolve injectable field '{field.Name}' of type '{field.FieldType.FullName}' on type '{targetType.FullName}'.");
                }

                if (!field.FieldType.IsInstanceOfType(value))
                {
                    throw new DependencyResolutionException(
                        $"Resolved service for injectable field '{field.Name}' on type '{targetType.FullName}' " +
                        $"is not assignable to '{field.FieldType.FullName}'.");
                }

                field.SetValue(target, value);
            }

            if (target is IInjected injected)
            {
                injected.OnInjected();
            }
        }

        private static FieldInfo[] GetInjectableFields(Type type)
        {
            lock (SyncLock)
            {
                if (FieldsByType.TryGetValue(type, out var fields))
                    return fields;

                fields = BuildInjectableFields(type);
                FieldsByType[type] = fields;
                return fields;
            }
        }

        private static FieldInfo[] BuildInjectableFields(Type type)
        {
            var fields = new List<FieldInfo>();

            for (var currentType = type; currentType != null && currentType != typeof(object); currentType = currentType.BaseType)
            {
                var declaredFields = currentType.GetFields(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.DeclaredOnly);

                foreach (var field in declaredFields)
                {
                    if (!field.IsDefined(typeof(InjectAttribute), false))
                        continue;

                    ValidateInjectableField(field, type);
                    fields.Add(field);
                }
            }

            return fields.ToArray();
        }

        private static void ValidateInjectableField(FieldInfo field, Type targetType)
        {
            if (field.IsStatic)
            {
                throw new DependencyResolutionException(
                    $"Injectable field '{field.Name}' on type '{targetType.FullName}' must be an instance field.");
            }

            if (field.IsLiteral)
            {
                throw new DependencyResolutionException(
                    $"Injectable field '{field.Name}' on type '{targetType.FullName}' must not be const.");
            }

            if (field.IsInitOnly)
            {
                throw new DependencyResolutionException(
                    $"Injectable field '{field.Name}' on type '{targetType.FullName}' must not be readonly.");
            }
        }
    }
}
