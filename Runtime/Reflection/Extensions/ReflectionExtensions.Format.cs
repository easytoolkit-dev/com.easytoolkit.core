using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Defines formatting options for converting member names to code strings.
    /// </summary>
    [Flags]
    public enum MemberFormat
    {
        /// <summary>
        /// No special formatting applied. Uses simple member names without modifiers or attributes.
        /// </summary>
        None = 0,

        /// <summary>
        /// Include access modifiers (public, private, protected, internal, etc.).
        /// </summary>
        IncludeModifiers = 1 << 0,

        /// <summary>
        /// Include custom attributes on the member.
        /// </summary>
        IncludeAttributes = 1 << 1,

        /// <summary>
        /// Include default value for fields with constant values.
        /// </summary>
        IncludeDefaultValue = 1 << 2,

        /// <summary>
        /// Include property accessor bodies (get/set implementations).
        /// </summary>
        IncludeBody = 1 << 3,

        /// <summary>
        /// Use C# type aliases instead of full type names (e.g., "int" instead of "System.Int32").
        /// </summary>
        UseTypeAliases = 1 << 4,

        /// <summary>
        /// Include full namespace in type names (e.g., "System.Collections.Generic.List&lt;T&gt;" instead of "List&lt;T&gt;").
        /// </summary>
        IncludeNamespace = 1 << 5,

        Default = IncludeModifiers | IncludeBody | UseTypeAliases | IncludeNamespace
    }

    public static partial class ReflectionExtensions
    {
        #region MemberInfo - Base ToCodeString

        /// <summary>
        /// Converts a member to its code representation with full formatting.
        /// </summary>
        /// <param name="member">The member to convert.</param>
        /// <returns>A string representation of the member.</returns>
        public static string ToCodeString([NotNull] this MemberInfo member)
        {
            return member.ToCodeString(MemberFormat.Default);
        }

        /// <summary>
        /// Converts a member to its code representation with specified formatting options.
        /// </summary>
        /// <param name="member">The member to convert.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A string representation of the member based on the specified format.</returns>
        /// <remarks>
        /// Dispatches to the appropriate ToCodeString implementation based on the member type.
        /// </remarks>
        public static string ToCodeString([NotNull] this MemberInfo member, MemberFormat format)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            switch (member)
            {
                case FieldInfo field:
                    return field.ToCodeString(format);

                case PropertyInfo property:
                    return property.ToCodeString(format);

                case MethodInfo method:
                    return method.ToCodeString(format);

                case ConstructorInfo constructor:
                    return constructor.ToCodeString(format);

                case EventInfo eventInfo:
                    return eventInfo.ToCodeString(format);

                default:
                    throw new NotSupportedException($"Member type '{member.MemberType}' is not supported by ToCodeString.");
            }
        }

        #endregion

        #region FieldInfo - ToCodeString

        /// <summary>
        /// Converts a field to its code representation with full formatting.
        /// </summary>
        /// <param name="field">The field to convert.</param>
        /// <returns>A string representation of the field.</returns>
        public static string ToCodeString([NotNull] this FieldInfo field)
        {
            return field.ToCodeString(MemberFormat.Default);
        }

        /// <summary>
        /// Converts a field to its code representation with specified formatting options.
        /// </summary>
        /// <param name="field">The field to convert.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A string representation of the field based on the specified format.</returns>
        /// <remarks>
        /// Example output: <c>private static readonly int _count = 0;</c>
        /// </remarks>
        public static string ToCodeString([NotNull] this FieldInfo field, MemberFormat format)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            var sb = new StringBuilder();
            var typeFormat = ToTypeFormat(format);

            // For const fields with default values, always use type aliases for better code readability
            if (field.IsLiteral && field.IsInitOnly == false && (format & MemberFormat.IncludeDefaultValue) != 0)
            {
                typeFormat |= TypeFormat.UseTypeAliases;
            }

            // Attributes
            sb.Append(FormatAttributes(field, format));

            // Modifiers
            // For const fields with default values, always include modifiers to properly display "public const int"
            var includeModifiers = (format & MemberFormat.IncludeModifiers) != 0 ||
                                   (field.IsLiteral && field.IsInitOnly == false && (format & MemberFormat.IncludeDefaultValue) != 0);

            if (includeModifiers)
            {
                sb.Append(GetAccessModifiers(field));
                if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
                    sb.Append(' ');
            }

            // Type and name
            sb.Append(field.FieldType.ToCodeString(typeFormat));
            sb.Append(' ');
            sb.Append(field.Name);

            // Default value for const fields
            if ((format & MemberFormat.IncludeDefaultValue) != 0 && field.IsLiteral && field.IsInitOnly == false)
            {
                var value = field.GetRawConstantValue();
                if (value != null)
                {
                    sb.Append(" = ");
                    sb.Append(FormatDefaultValue(value, field.FieldType));
                }
            }

            sb.Append(';');

            return sb.ToString();
        }

        #endregion

        #region PropertyInfo - ToCodeString

        /// <summary>
        /// Converts a property to its code representation with full formatting.
        /// </summary>
        /// <param name="property">The property to convert.</param>
        /// <returns>A string representation of the property.</returns>
        public static string ToCodeString([NotNull] this PropertyInfo property)
        {
            return property.ToCodeString(MemberFormat.Default);
        }

        /// <summary>
        /// Converts a property to its code representation with specified formatting options.
        /// </summary>
        /// <param name="property">The property to convert.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A string representation of the property based on the specified format.</returns>
        /// <remarks>
        /// Example output: <c>public int Count { get; private set; }</c>
        /// </remarks>
        public static string ToCodeString([NotNull] this PropertyInfo property, MemberFormat format)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var sb = new StringBuilder();
            var typeFormat = ToTypeFormat(format);

            // Attributes
            sb.Append(FormatAttributes(property, format));

            // Modifiers
            if ((format & MemberFormat.IncludeModifiers) != 0)
            {
                sb.Append(GetAccessModifiers(property));
                if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
                    sb.Append(' ');
            }

            // Type and name
            sb.Append(property.PropertyType.ToCodeString(typeFormat));
            sb.Append(' ');
            sb.Append(property.Name);

            // Accessors
            sb.Append(" { ");

            var getMethod = property.GetMethod;
            var setMethod = property.SetMethod;

            if (getMethod != null)
            {
                var getAccess = (format & MemberFormat.IncludeModifiers) != 0
                    ? GetAccessLevel(getMethod) + " "
                    : string.Empty;
                sb.Append(getAccess);
                sb.Append("get; ");
            }

            if (setMethod != null)
            {
                var setAccess = (format & MemberFormat.IncludeModifiers) != 0
                    ? GetAccessLevel(setMethod) + " "
                    : string.Empty;

                // Check if init-only (C# 9.0)
                var isInitOnly = setMethod.ReturnParameter.GetRequiredCustomModifiers()
                    .Any(m => m.FullName == "System.Runtime.CompilerServices.IsExternalInit");
                sb.Append(setAccess);
                sb.Append(isInitOnly ? "init; " : "set; ");
            }

            sb.Append('}');

            return sb.ToString();
        }

        #endregion

        #region MethodInfo - ToCodeString

        /// <summary>
        /// Converts a method to its code representation with full formatting.
        /// </summary>
        /// <param name="method">The method to convert.</param>
        /// <returns>A string representation of the method.</returns>
        public static string ToCodeString([NotNull] this MethodInfo method)
        {
            return method.ToCodeString(MemberFormat.Default);
        }

        /// <summary>
        /// Converts a method to its code representation with specified formatting options.
        /// </summary>
        /// <param name="method">The method to convert.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A string representation of the method based on the specified format.</returns>
        /// <remarks>
        /// Example output: <c>public void Process&lt;T&gt;(T input, ref int count) where T : class</c>
        /// </remarks>
        public static string ToCodeString([NotNull] this MethodInfo method, MemberFormat format)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            var sb = new StringBuilder();
            var typeFormat = ToTypeFormat(format);

            // Attributes
            sb.Append(FormatAttributes(method, format));

            // Modifiers
            if ((format & MemberFormat.IncludeModifiers) != 0)
            {
                sb.Append(GetAccessModifiers(method));
                if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
                    sb.Append(' ');
            }

            // Handle virtual/override/abstract modifiers
            if ((format & MemberFormat.IncludeModifiers) != 0)
            {
                // Check abstract first, as abstract methods may have IsVirtual = true in metadata
                if (method.IsAbstract)
                    sb.Append("abstract ");
                else if (method.IsVirtual && !method.IsFinal)
                    sb.Append("virtual ");
                else if (method.IsFinal && method.IsVirtual)
                    sb.Append("sealed ");
            }

            // Return type
            if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
                sb.Append(' ');
            sb.Append(method.ReturnType.ToCodeString(typeFormat));
            sb.Append(' ');

            // Generic type parameters
            if (method.IsGenericMethod)
            {
                var genericArgs = method.GetGenericArguments();
                sb.Append(method.Name);
                sb.Append('<');
                sb.Append(string.Join(", ", genericArgs.Select(t => t.Name)));
                sb.Append('>');
            }
            else
            {
                sb.Append(method.Name);
            }

            // Parameters
            sb.Append('(');
            var parameters = method.GetParameters();
            sb.Append(FormatParameters(parameters, format));
            sb.Append(')');

            // Generic constraints
            if (method.IsGenericMethod && (format & MemberFormat.IncludeModifiers) != 0)
            {
                sb.Append(FormatGenericConstraints(method));
            }

            sb.Append(';');

            return sb.ToString();
        }

        #endregion

        #region ConstructorInfo - ToCodeString

        /// <summary>
        /// Converts a constructor to its code representation with full formatting.
        /// </summary>
        /// <param name="constructor">The constructor to convert.</param>
        /// <returns>A string representation of the constructor.</returns>
        public static string ToCodeString([NotNull] this ConstructorInfo constructor)
        {
            return constructor.ToCodeString(MemberFormat.Default);
        }

        /// <summary>
        /// Converts a constructor to its code representation with specified formatting options.
        /// </summary>
        /// <param name="constructor">The constructor to convert.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A string representation of the constructor based on the specified format.</returns>
        /// <remarks>
        /// Example output: <c>public MyClass(string name, int value)</c>
        /// </remarks>
        public static string ToCodeString([NotNull] this ConstructorInfo constructor, MemberFormat format)
        {
            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            var sb = new StringBuilder();
            var typeFormat = ToTypeFormat(format);

            // Attributes
            sb.Append(FormatAttributes(constructor, format));

            // Modifiers
            if ((format & MemberFormat.IncludeModifiers) != 0)
            {
                sb.Append(GetAccessModifiers(constructor));
                if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
                    sb.Append(' ');
            }

            // Constructor name (declaring type name)
            var declaringTypeName = constructor.DeclaringType?.ToCodeString(typeFormat) ?? "UnknownType";
            sb.Append(declaringTypeName);

            // Parameters
            sb.Append('(');
            var parameters = constructor.GetParameters();
            sb.Append(FormatParameters(parameters, format));
            sb.Append(')');

            sb.Append(';');

            return sb.ToString();
        }

        #endregion

        #region EventInfo - ToCodeString

        /// <summary>
        /// Converts an event to its code representation with full formatting.
        /// </summary>
        /// <param name="eventInfo">The event to convert.</param>
        /// <returns>A string representation of the event.</returns>
        public static string ToCodeString([NotNull] this EventInfo eventInfo)
        {
            return eventInfo.ToCodeString(MemberFormat.Default);
        }

        /// <summary>
        /// Converts an event to its code representation with specified formatting options.
        /// </summary>
        /// <param name="eventInfo">The event to convert.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A string representation of the event based on the specified format.</returns>
        /// <remarks>
        /// Example output: <c>public event EventHandler Clicked</c>
        /// </remarks>
        public static string ToCodeString([NotNull] this EventInfo eventInfo, MemberFormat format)
        {
            if (eventInfo == null)
                throw new ArgumentNullException(nameof(eventInfo));

            var sb = new StringBuilder();
            var typeFormat = ToTypeFormat(format);

            // Attributes
            sb.Append(FormatAttributes(eventInfo, format));

            // Modifiers
            if ((format & MemberFormat.IncludeModifiers) != 0)
            {
                sb.Append(GetAccessModifiers(eventInfo));
                if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
                    sb.Append(' ');
            }

            sb.Append("event ");
            sb.Append(eventInfo.EventHandlerType.ToCodeString(typeFormat));
            sb.Append(' ');
            sb.Append(eventInfo.Name);
            sb.Append(';');

            return sb.ToString();
        }

        #endregion


        #region MemberFormat to TypeFormat Conversion

        /// <summary>
        /// Converts a <see cref="MemberFormat"/> to the corresponding <see cref="TypeFormat"/>.
        /// </summary>
        /// <param name="memberFormat">The member format to convert.</param>
        /// <returns>The corresponding type format.</returns>
        private static TypeFormat ToTypeFormat(MemberFormat memberFormat)
        {
            var typeFormat = TypeFormat.None;
            if ((memberFormat & MemberFormat.UseTypeAliases) != 0)
                typeFormat |= TypeFormat.UseTypeAliases;
            if ((memberFormat & MemberFormat.IncludeNamespace) != 0)
                typeFormat |= TypeFormat.IncludeNamespace;
            return typeFormat;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the access modifier string for a member.
        /// </summary>
        /// <param name="member">The member to get access modifiers for.</param>
        /// <returns>The access modifier string (e.g., "public", "private", etc.).</returns>
        private static string GetAccessModifiers([NotNull] this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            var sb = new StringBuilder();

            // Handle static modifier
            if (member is FieldInfo field && field.IsStatic)
                sb.Append("static ");
            else if (member is PropertyInfo property && property.GetMethod?.IsStatic == true)
                sb.Append("static ");
            else if (member is MethodInfo method && method.IsStatic)
                sb.Append("static ");
            else if (member is EventInfo eventInfo && eventInfo.AddMethod?.IsStatic == true)
                sb.Append("static ");

            // Handle readonly modifier for fields
            if (member is FieldInfo fieldInfo && fieldInfo.IsInitOnly)
                sb.Append("readonly ");

            // Handle const modifier for fields
            if (member is FieldInfo constField && constField.IsLiteral)
                sb.Append("const ");

            // Handle access level
            var accessLevel = GetAccessLevel(member);
            if (!string.IsNullOrEmpty(accessLevel))
                sb.Append(accessLevel).Append(' ');

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Gets the access level string for a member.
        /// </summary>
        /// <param name="member">The member to get access level for.</param>
        /// <returns>The access level string (e.g., "public", "private", "protected", "internal").</returns>
        private static string GetAccessLevel([NotNull] MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            // For methods, properties, events
            if (member is MethodInfo method)
            {
                if (method.IsPublic) return "public";
                if (method.IsPrivate) return "private";
                if (method.IsFamily) return "protected";
                if (method.IsAssembly) return "internal";
                if (method.IsFamilyOrAssembly) return "protected internal";
                if (method.IsFamilyAndAssembly) return "private protected";
            }

            if (member is PropertyInfo property)
            {
                var getMethod = property.GetMethod;
                var setMethod = property.SetMethod;

                if (getMethod != null && setMethod != null)
                {
                    // Both accessors exist, use the more visible one
                    var getAccess = GetAccessLevel(getMethod);
                    var setAccess = GetAccessLevel(setMethod);
                    return GetMoreVisibleAccess(getAccess, setAccess);
                }
                else if (getMethod != null)
                {
                    return GetAccessLevel(getMethod);
                }
                else if (setMethod != null)
                {
                    return GetAccessLevel(setMethod);
                }
            }

            if (member is EventInfo eventInfo)
            {
                var addMethod = eventInfo.AddMethod;
                if (addMethod != null)
                    return GetAccessLevel(addMethod);
            }

            if (member is FieldInfo field)
            {
                if (field.IsPublic) return "public";
                if (field.IsPrivate) return "private";
                if (field.IsFamily) return "protected";
                if (field.IsAssembly) return "internal";
                if (field.IsFamilyOrAssembly) return "protected internal";
                if (field.IsFamilyAndAssembly) return "private protected";
            }

            if (member is ConstructorInfo constructor)
            {
                if (constructor.IsPublic) return "public";
                if (constructor.IsPrivate) return "private";
                if (constructor.IsFamily) return "protected";
                if (constructor.IsAssembly) return "internal";
                if (constructor.IsFamilyOrAssembly) return "protected internal";
                if (constructor.IsFamilyAndAssembly) return "private protected";
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the more visible of two access levels.
        /// </summary>
        private static string GetMoreVisibleAccess(string access1, string access2)
        {
            // Priority order (most visible to least visible)
            var visibilityOrder = new[] { "public", "protected internal", "internal", "protected", "private protected", "private" };

            var index1 = Array.IndexOf(visibilityOrder, access1);
            var index2 = Array.IndexOf(visibilityOrder, access2);

            if (index1 < 0) return access2;
            if (index2 < 0) return access1;

            return visibilityOrder[Math.Min(index1, index2)];
        }

        /// <summary>
        /// Formats custom attributes on a member.
        /// </summary>
        private static string FormatAttributes([NotNull] MemberInfo member, MemberFormat format)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if ((format & MemberFormat.IncludeAttributes) == 0)
                return string.Empty;

            var attributes = member.GetCustomAttributesData();
            if (attributes.Count == 0)
                return string.Empty;

            return FormatAttributeList(attributes, format);
        }

        /// <summary>
        /// Formats custom attributes on a parameter.
        /// </summary>
        private static string FormatAttributes([NotNull] ParameterInfo parameter, MemberFormat format)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            if ((format & MemberFormat.IncludeAttributes) == 0)
                return string.Empty;

            var attributes = parameter.GetCustomAttributesData();
            if (attributes.Count == 0)
                return string.Empty;

            return FormatAttributeList(attributes, format);
        }

        /// <summary>
        /// Formats a list of custom attributes.
        /// </summary>
        private static string FormatAttributeList(IList<CustomAttributeData> attributes, MemberFormat format)
        {
            var sb = new StringBuilder();
            var typeFormat = ToTypeFormat(format);

            foreach (var attr in attributes)
            {
                sb.Append('[');
                sb.Append(attr.AttributeType.ToCodeString(typeFormat));

                if (attr.ConstructorArguments.Count > 0)
                {
                    sb.Append('(');
                    var args = attr.ConstructorArguments
                        .Select(a => FormatAttributeArgument(a))
                        .ToArray();
                    sb.Append(string.Join(", ", args));
                    sb.Append(')');
                }

                sb.Append("] ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Formats an attribute constructor argument.
        /// </summary>
        private static string FormatAttributeArgument(CustomAttributeTypedArgument arg)
        {
            if (arg.Value == null)
                return "null";

            if (arg.Value is Type type)
                return $"typeof({type.ToCodeString(TypeFormat.UseTypeAliases)})";

            if (arg.Value.GetType().IsArray)
            {
                var array = (CustomAttributeTypedArgument[])arg.Value;
                return $"new[] {{ {string.Join(", ", array.Select(FormatAttributeArgument))} }}";
            }

            if (arg.Value is string str)
                return $"\"{str}\"";

            if (arg.Value is bool b)
                return b ? "true" : "false";

            return arg.Value.ToString();
        }

        /// <summary>
        /// Formats method parameters.
        /// </summary>
        private static string FormatParameters(ParameterInfo[] parameters, MemberFormat format)
        {
            if (parameters.Length == 0)
                return string.Empty;

            var typeFormat = ToTypeFormat(format);
            var sb = new StringBuilder();

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];

                // Handle attributes on parameters
                if ((format & MemberFormat.IncludeAttributes) != 0)
                {
                    var paramAttributes = FormatAttributes(param, format);
                    sb.Append(paramAttributes);
                }

                // Handle modifiers
                if (param.IsOut)
                    sb.Append("out ");
                else if (param.ParameterType.IsByRef)
                    sb.Append("ref ");
                else if (param.IsIn)
                    sb.Append("in ");
                else if (param.GetCustomAttribute<ParamArrayAttribute>() != null)
                    sb.Append("params ");

                // Handle type
                var paramType = param.ParameterType;
                if (paramType.IsByRef)
                    paramType = paramType.GetElementType();

                sb.Append(paramType.ToCodeString(typeFormat));

                // Parameter name
                sb.Append(' ');
                sb.Append(param.Name);

                // Handle default value
                if ((format & MemberFormat.IncludeDefaultValue) != 0)
                {
                    if (param.HasDefaultValue)
                    {
                        sb.Append(" = ");
                        sb.Append(FormatDefaultValue(param.DefaultValue, paramType));
                    }
                }

                if (i < parameters.Length - 1)
                    sb.Append(", ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Formats a default value for a parameter.
        /// </summary>
        private static string FormatDefaultValue(object value, Type type)
        {
            if (value == null)
                return "null";

            if (type == typeof(string))
                return $"\"{value}\"";

            if (type == typeof(char))
                return $"'{value}'";

            if (value is bool b)
                return b ? "true" : "false";

            if (value.GetType().IsEnum)
                return $"{type.ToCodeString(TypeFormat.UseTypeAliases)}.{value}";

            return value.ToString();
        }

        /// <summary>
        /// Formats generic type constraints.
        /// </summary>
        private static string FormatGenericConstraints(MethodInfo method)
        {
            if (!method.IsGenericMethod)
                return string.Empty;

            var genericArgs = method.GetGenericArguments();
            var sb = new StringBuilder();

            foreach (var arg in genericArgs)
            {
                var constraints = arg.GetGenericParameterConstraints();
                var hasAnyConstraint = constraints.Length > 0 ||
                    arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint) ||
                    arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint) ||
                    arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);

                if (!hasAnyConstraint)
                    continue;

                sb.Append(" where ");
                sb.Append(arg.Name);
                sb.Append(" : ");

                var constraintList = new System.Collections.Generic.List<string>();

                // Handle struct constraint
                if (arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
                {
                    constraintList.Add("struct");
                }

                // Handle class constraint
                if (arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                {
                    constraintList.Add("class");
                }

                // Handle new() constraint
                if (arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                {
                    constraintList.Add("new()");
                }

                // Handle type constraints
                foreach (var constraint in constraints)
                {
                    if (constraint != typeof(object))
                    {
                        constraintList.Add(constraint.ToCodeString(TypeFormat.UseTypeAliases));
                    }
                }

                sb.Append(string.Join(", ", constraintList));
            }

            return sb.ToString();
        }

        #endregion

    }
}
