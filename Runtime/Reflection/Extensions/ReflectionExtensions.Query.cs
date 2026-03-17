using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection
{
    public static partial class ReflectionExtensions
    {
        /// <summary>
        /// Tries to get the type of the specified member.
        /// </summary>
        /// <param name="member">The member to get the type from.</param>
        /// <param name="memberType">When this method returns, contains the type of the member if successful; otherwise, <c>null</c>.</param>
        /// <returns>
        /// <c>true</c> if the member type was successfully retrieved; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGetMemberType([NotNull] this MemberInfo member, out Type memberType)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (member is FieldInfo field)
            {
                memberType = field.FieldType;
                return true;
            }

            if (member is PropertyInfo property)
            {
                memberType = property.PropertyType;
                return true;
            }

            if (member is MethodInfo method)
            {
                memberType = method.ReturnType;
                return true;
            }

            memberType = null;
            return false;
        }

        public static Type GetMemberType([NotNull] this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (member.TryGetMemberType(out var memberType))
            {
                return memberType;
            }

            throw new NotSupportedException($"Member type '{member.MemberType}' is not supported.");
        }

        /// <summary>
        /// Gets the property associated with a backing field, if one exists.
        /// </summary>
        /// <param name="field">The backing field.</param>
        /// <returns>
        /// The property associated with the backing field, or <c>null</c> if the field is not a backing field.
        /// </returns>
        public static PropertyInfo GetAssociatedProperty([NotNull] this FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            var fieldName = field.Name;

            // Extract property name from compiler-generated backing field
            if (fieldName.StartsWith("<") && fieldName.EndsWith(">k__BackingField"))
            {
                var propertyName = fieldName.Substring(1, fieldName.IndexOf('>') - 1);
                return field.DeclaringType.GetProperty(propertyName,
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            // Check for common manual backing field conventions
            var properties = field.DeclaringType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var property in properties)
            {
                var propertyName = property.Name;

                if (fieldName == $"_{propertyName}" ||
                    fieldName == $"m_{propertyName}" ||
                    fieldName == $"{propertyName}_" ||
                    fieldName == $"{propertyName}BackingField")
                {
                    return property;
                }
            }

            return null;
        }
    }
}
