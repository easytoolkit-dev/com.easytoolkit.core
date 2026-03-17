using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
{
    public static partial class ReflectionExtensions
    {
        public static bool IsDefined<T>(this MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.IsDefined(typeof(T), inherit);
        }

        public static bool IsDefined<T>(this Type type, bool inherit = false, bool includeInterface = false)
            where T : Attribute
        {
            if (type.IsDefined(typeof(T), inherit))
                return true;

            if (!includeInterface)
                return false;

            var queue = new Queue<Type>(type.GetInterfaces());

            while (queue.Count > 0)
            {
                var iface = queue.Dequeue();

                if (iface.IsDefined(typeof(T)))
                    return true;

                foreach (var sub in iface.GetInterfaces())
                    queue.Enqueue(sub);
            }

            return false;
        }

        /// <summary>
        /// Determines whether the member is static.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>
        /// <c>true</c> if the member is static; otherwise, <c>false</c>.
        /// For properties and events, returns <c>true</c> if at least one accessor is static.
        /// </returns>
        /// <exception cref="NotSupportedException">Thrown when the member type is not supported.</exception>
        public static bool IsStaticMember([NotNull] this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (member is FieldInfo field)
            {
                return field.IsStatic;
            }

            if (member is PropertyInfo property)
            {
                return property.GetMethod?.IsStatic == true || property.SetMethod?.IsStatic == true;
            }

            if (member is MethodInfo method)
            {
                return method.IsStatic;
            }

            if (member is EventInfo eventInfo)
            {
                return eventInfo.AddMethod?.IsStatic == true || eventInfo.RemoveMethod?.IsStatic == true;
            }

            if (member is ConstructorInfo)
            {
                return true;
            }

            throw new NotSupportedException($"Member type '{member.MemberType}' is not supported.");
        }

        /// <summary>
        /// Determines whether the field is a backing field for a property.
        /// </summary>
        /// <param name="field">The field to check.</param>
        /// <returns>
        /// <c>true</c> if the field is a backing field for a compiler-generated property;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method detects compiler-generated backing fields (with names like "&lt;PropertyName&gt;k__BackingField").
        /// </remarks>
        public static bool IsBackingField([NotNull] this FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            var fieldName = field.Name;

            // Check for compiler-generated auto-property backing field
            if (fieldName.StartsWith("<") && fieldName.EndsWith(">k__BackingField"))
            {
                return true;
            }

            return false;
        }
    }
}
