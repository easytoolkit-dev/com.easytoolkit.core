using System;

namespace EasyToolKit.Core.Reflection
{
    public static partial class TypeExtensions
    {
        public static bool TryInferTypeArguments(this Type genericTypeDefinition,
            Type[] inputTypeArguments,
            out Type[] inferredTypes)
        {
            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));

            return TypeAnalyzerFactory.GetGenericTypeDefinitionAnalyzer(genericTypeDefinition)
                .TryInferTypeArguments(inputTypeArguments,out inferredTypes);
        }
    }
}
