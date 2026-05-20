using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Performs field injection for services that use injection marker attributes.
    /// </summary>
    internal static class ServiceInjector
    {
        private static readonly Dictionary<InjectableFieldCacheKey, FieldInfo[]> FieldsByCacheKey = new();
        private static readonly object SyncLock = new();

        /// <summary>
        /// Injects marked fields on the specified target.
        /// </summary>
        public static void Inject(IServiceProvider provider, object target)
        {
            Inject(provider, target, typeof(InjectAttribute));
        }

        /// <summary>
        /// Injects fields marked with the specified attribute type on the specified target.
        /// </summary>
        public static void Inject(IServiceProvider provider, object target, Type injectAttributeType)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            ValidateInjectAttributeType(injectAttributeType);

            var targetType = target.GetType();
            var fields = GetInjectableFields(targetType, injectAttributeType);

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

        private static FieldInfo[] GetInjectableFields(Type type, Type injectAttributeType)
        {
            var cacheKey = new InjectableFieldCacheKey(type, injectAttributeType);

            lock (SyncLock)
            {
                if (FieldsByCacheKey.TryGetValue(cacheKey, out var fields))
                    return fields;

                fields = BuildInjectableFields(type, injectAttributeType);
                FieldsByCacheKey[cacheKey] = fields;
                return fields;
            }
        }

        private static FieldInfo[] BuildInjectableFields(Type type, Type injectAttributeType)
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
                    if (!field.IsDefined(injectAttributeType, false))
                        continue;

                    ValidateInjectableField(field, type);
                    fields.Add(field);
                }
            }

            return fields.ToArray();
        }

        private static void ValidateInjectAttributeType(Type injectAttributeType)
        {
            if (injectAttributeType == null)
                throw new ArgumentNullException(nameof(injectAttributeType));

            if (!typeof(Attribute).IsAssignableFrom(injectAttributeType))
            {
                throw new ArgumentException(
                    $"Injection marker type '{injectAttributeType.FullName}' must derive from '{typeof(Attribute).FullName}'.",
                    nameof(injectAttributeType));
            }
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

        private readonly struct InjectableFieldCacheKey : IEquatable<InjectableFieldCacheKey>
        {
            private readonly Type _targetType;
            private readonly Type _injectAttributeType;

            public InjectableFieldCacheKey(Type targetType, Type injectAttributeType)
            {
                _targetType = targetType;
                _injectAttributeType = injectAttributeType;
            }

            public bool Equals(InjectableFieldCacheKey other)
            {
                return _targetType == other._targetType && _injectAttributeType == other._injectAttributeType;
            }

            public override bool Equals(object obj)
            {
                return obj is InjectableFieldCacheKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_targetType != null ? _targetType.GetHashCode() : 0) * 397)
                        ^ (_injectAttributeType != null ? _injectAttributeType.GetHashCode() : 0);
                }
            }
        }
    }
}
