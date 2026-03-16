using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection
{
    public static class ReflectionExtensions
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


        public static IEnumerable<Type> GetAllTypes(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes());
        }

        public static Type FindType(this IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            return assemblies.GetAllTypes().FirstOrDefault(predicate);
        }

        public static Type FindTypeByName(this IEnumerable<Assembly> assemblies, string fullName)
        {
            return assemblies.GetAllTypes().FirstOrDefault(t => t.FullName == fullName);
        }

        public static string GetSignature(this MemberInfo member)
        {
            var sb = new StringBuilder();

            // Append the member type (e.g., Method, Property, Field, etc.)
            sb.Append(member.MemberType.ToString());
            sb.Append(" ");

            // Append the member's declaring type (including namespace)
            sb.Append(member.DeclaringType.FullName);
            sb.Append(".");

            // Append the member name
            sb.Append(member.Name);

            // If the member is a method, append parameter types and return type
            if (member is MethodInfo methodInfo)
            {
                sb.Append($"({GetMethodParametersSignature(methodInfo)}) : ");
                sb.Append(methodInfo.ReturnType.FullName);
            }
            else if (member is PropertyInfo propertyInfo)
            {
                // If the member is a property, append the property type
                sb.Append(" : ");
                sb.Append(propertyInfo.PropertyType.FullName);
            }
            else if (member is FieldInfo fieldInfo)
            {
                // If the member is a field, append the field type
                sb.Append(" : ");
                sb.Append(fieldInfo.FieldType.FullName);
            }
            else if (member is EventInfo eventInfo)
            {
                // If the member is an event, append the event handler type
                sb.Append(" : ");
                sb.Append(eventInfo.EventHandlerType.FullName);
            }

            return sb.ToString();
        }


        public static string GetMethodParametersSignature(this MethodInfo method)
        {
            return string.Join(", ",
                method.GetParameters().Select(x => $"{x.ParameterType.GetAliases()} {x.Name}"));
        }

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
