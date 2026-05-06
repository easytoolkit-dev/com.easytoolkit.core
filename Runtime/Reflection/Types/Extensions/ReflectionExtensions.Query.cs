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
        /// Tries to find the backing field for this property.
        /// </summary>
        /// <param name="property">The property to search.</param>
        /// <param name="backingField">When this method returns, contains the backing field if found; otherwise, <c>null</c>.</param>
        /// <param name="strict">
        /// If <c>true</c>, only searches for compiler-generated backing fields.
        /// If <c>false</c>, also attempts common naming patterns. Default is <c>true</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if a backing field was found; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// When <paramref name="strict"/> is <c>true</c>, this method only searches for:
        /// <list type="bullet">
        /// <item><description>&lt;PropertyName&gt;k__BackingField - Standard auto-property backing field</description></item>
        /// <item><description>&lt;PropertyName&gt;i__Field - Compiler-generated field (e.g., anonymous types, async state machines)</description></item>
        /// </list>
        /// When <paramref name="strict"/> is <c>false</c>, this method also attempts:
        /// <list type="bullet">
        /// <item>Underscore prefix: <c>_propertyName</c></item>
        /// <item>CamelCase with 'm' prefix: <c>m_propertyName</c></item>
        /// <item>Same name as property (case-sensitive)</item>
        /// </list>
        /// Note that compiler-generated backing fields may not be accessible due to their visibility.
        /// </remarks>
        public static bool TryGetBackingField([NotNull] this PropertyInfo property, out FieldInfo backingField, bool strict = true)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var declaringType = property.DeclaringType;
            if (declaringType == null)
            {
                backingField = null;
                return false;
            }

            // Try compiler-generated auto-property backing field name
            var compilerFieldName = $"<{property.Name}>k__BackingField";
            backingField = declaringType.GetField(compilerFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (backingField != null)
            {
                return true;
            }

            // Try other compiler-generated field formats
            var compilerFieldName2 = $"<{property.Name}>i__Field";
            backingField = declaringType.GetField(compilerFieldName2, BindingFlags.Instance | BindingFlags.NonPublic);
            if (backingField != null)
            {
                return true;
            }

            // In strict mode, only search for compiler-generated backing fields
            if (strict)
            {
                backingField = null;
                return false;
            }

            // Try common naming patterns
            var possibleNames = new List<string>
            {
                $"_{char.ToLower(property.Name[0])}{property.Name[1..]}", // _propertyName
                $"m_{property.Name}", // m_PropertyName
                $"m{char.ToUpper(property.Name[0])}{property.Name[1..]}", // m_PropertyName
                property.Name // PropertyName
            };

            foreach (var name in possibleNames)
            {
                backingField = declaringType.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (backingField != null)
                {
                    return true;
                }
            }

            backingField = null;
            return false;
        }

        /// <summary>
        /// Tries to find the property associated with this field.
        /// </summary>
        /// <param name="field">The field to search.</param>
        /// <param name="associatedProperty">When this method returns, contains the associated property if found; otherwise, <c>null</c>.</param>
        /// <param name="strict">
        /// If <c>true</c>, only matches compiler-generated backing fields with their properties.
        /// If <c>false</c>, also attempts common naming pattern matching. Default is <c>true</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if an associated property was found; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// When <paramref name="strict"/> is <c>true</c>, this method only matches:
        /// <list type="bullet">
        /// <item><description>&lt;PropertyName&gt;k__BackingField - Standard auto-property backing field</description></item>
        /// <item><description>&lt;FieldName&gt;i__Field - Compiler-generated field (e.g., anonymous types, async state machines)</description></item>
        /// </list>
        /// When <paramref name="strict"/> is <c>false</c>, this method also attempts:
        /// <list type="bullet">
        /// <item>Underscore prefix: <c>_propertyName</c> → <c>PropertyName</c></item>
        /// <item>CamelCase with 'm' prefix: <c>m_propertyName</c> → <c>PropertyName</c></item>
        /// <item>Same name as property (case-sensitive)</item>
        /// </list>
        /// </remarks>
        public static bool TryGetAssociatedProperty([NotNull] this FieldInfo field, out PropertyInfo associatedProperty, bool strict = true)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            var declaringType = field.DeclaringType;
            if (declaringType == null)
            {
                associatedProperty = null;
                return false;
            }

            var fieldName = field.Name;

            // Try compiler-generated auto-property backing field name
            if (fieldName.StartsWith("<") && fieldName.EndsWith(">k__BackingField"))
            {
                var propertyName = fieldName[1..^16];
                associatedProperty = declaringType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return associatedProperty != null;
            }

            // Try other compiler-generated field formats
            if (fieldName.StartsWith("<") && fieldName.EndsWith(">i__Field"))
            {
                var propertyName = fieldName[1..^9];
                associatedProperty = declaringType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return associatedProperty != null;
            }

            // In strict mode, only match compiler-generated backing fields
            if (strict)
            {
                associatedProperty = null;
                return false;
            }

            {
                // Try common naming patterns
                var propertyName = fieldName switch
                {
                    // _propertyName → PropertyName
                    _ when fieldName.StartsWith("_") && fieldName.Length > 1
                        => char.ToUpper(fieldName[1]) + (fieldName.Length > 2 ? fieldName[2..] : ""),
                    // m_PropertyName → PropertyName
                    _ when fieldName.StartsWith("m_") && fieldName.Length > 2
                        => char.ToUpper(fieldName[2]) + (fieldName.Length > 3 ? fieldName[3..] : ""),
                    // m_PropertyName (capitalized after m) → PropertyName
                    _ when fieldName.StartsWith("m") && fieldName.Length > 1 && char.IsUpper(fieldName[1])
                        => fieldName[1..],
                    // Assume field name matches property name
                    _ => fieldName
                };

                associatedProperty = declaringType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return associatedProperty != null;
            }
        }
    }
}
