using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Reflection.Tests
{
    /// <summary>
    /// Unit tests for TypeUtility components.
    /// </summary>
    public class TestTypeUtility
    {
        #region GetTypeName Tests

        /// <summary>
        /// Verifies that GetTypeName returns assembly-qualified name for standard types.
        /// </summary>
        [Test]
        public void GetTypeName_SystemType_ReturnsAssemblyQualifiedName()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var result = TypeUtility.GetTypeName(type);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("System.String"));
        }

        /// <summary>
        /// Verifies that GetTypeName returns consistent result on multiple calls.
        /// </summary>
        [Test]
        public void GetTypeName_CalledMultipleTimes_ReturnsSameResult()
        {
            // Arrange
            var type = typeof(List<int>);

            // Act
            var result1 = TypeUtility.GetTypeName(type);
            var result2 = TypeUtility.GetTypeName(type);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        /// <summary>
        /// Verifies that GetTypeName throws ArgumentNullException for null type.
        /// </summary>
        [Test]
        public void GetTypeName_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => TypeUtility.GetTypeName(type));
            Assert.AreEqual("type", ex.ParamName);
        }

        /// <summary>
        /// Verifies that GetTypeName handles generic types correctly.
        /// </summary>
        [Test]
        public void GetTypeName_GenericType_ReturnsValidTypeName()
        {
            // Arrange
            var type = typeof(Dictionary<string, int>);

            // Act
            var result = TypeUtility.GetTypeName(type);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Dictionary"));
        }

        #endregion

        #region FindType Tests

        /// <summary>
        /// Verifies that FindType returns null for null or empty type name.
        /// </summary>
        [Test]
        public void FindType_NullTypeName_ThrowsArgumentNullException()
        {
            // Arrange
            string typeName = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TypeUtility.FindType(typeName));
        }

        /// <summary>
        /// Verifies that FindType returns null for empty type name.
        /// </summary>
        [Test]
        public void FindType_EmptyTypeName_ThrowsArgumentNullException()
        {
            // Arrange
            string typeName = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TypeUtility.FindType(typeName));
        }

        /// <summary>
        /// Verifies that FindType returns null for whitespace type name.
        /// </summary>
        [Test]
        public void FindType_WhitespaceTypeName_ThrowsArgumentNullException()
        {
            // Arrange
            string typeName = "   ";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TypeUtility.FindType(typeName));
        }

        /// <summary>
        /// Verifies that FindType finds system type by simple name.
        /// </summary>
        [Test]
        public void FindType_SystemStringSimpleName_ReturnsStringType()
        {
            // Arrange
            string typeName = "System.String";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(string), result);
        }

        /// <summary>
        /// Verifies that FindType finds system type by assembly-qualified name.
        /// </summary>
        [Test]
        public void FindType_SystemIntAssemblyQualifiedName_ReturnsIntType()
        {
            // Arrange
            string typeName = typeof(int).AssemblyQualifiedName;

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int), result);
        }

        /// <summary>
        /// Verifies that FindType returns null for non-existent type.
        /// </summary>
        [Test]
        public void FindType_NonExistentType_ReturnsNull()
        {
            // Arrange
            string typeName = "NonExistent.Type.Name";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies that FindType caches results for subsequent calls.
        /// </summary>
        [Test]
        public void FindType_CalledMultipleTimes_ReturnsSameResult()
        {
            // Arrange
            string typeName = "System.Int32";

            // Act
            var result1 = TypeUtility.FindType(typeName);
            var result2 = TypeUtility.FindType(typeName);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        /// <summary>
        /// Verifies that FindType handles generic List type correctly.
        /// </summary>
        [Test]
        public void FindType_GenericListType_ReturnsCorrectType()
        {
            // Arrange
            string typeName = typeof(List<string>).AssemblyQualifiedName;

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<string>), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic Dictionary type correctly.
        /// </summary>
        [Test]
        public void FindType_GenericDictionaryType_ReturnsCorrectType()
        {
            // Arrange
            string typeName = typeof(Dictionary<int, string>).AssemblyQualifiedName;

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<int, string>), result);
        }

        /// <summary>
        /// Verifies that FindType returns cached null result for previously searched non-existent type.
        /// </summary>
        [Test]
        public void FindType_NonExistentTypeTwice_ReturnsNullBothTimes()
        {
            // Arrange
            string typeName = "TotallyFakeType.That.DoesNotExist";

            // Act
            var result1 = TypeUtility.FindType(typeName);
            var result2 = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        /// <summary>
        /// Verifies that FindType handles generic Dictionary without main assembly information.
        /// </summary>
        [Test]
        public void FindType_GenericDictionaryWithoutMainAssembly_ReturnsCorrectType()
        {
            // Arrange
            // Format: Generic type without assembly info for the main type
            string typeName = "System.Collections.Generic.Dictionary`2[[System.String],[System.Int32]]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic List without main assembly information.
        /// </summary>
        [Test]
        public void FindType_GenericListWithoutMainAssembly_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List`1[[System.String]]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<string>), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic type with type arguments missing assembly information.
        /// </summary>
        [Test]
        public void FindType_GenericTypeWithArgumentsWithoutAssembly_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Int32, mscorlib]], mscorlib";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles nested generic types without assembly information.
        /// </summary>
        [Test]
        public void FindType_NestedGenericTypeWithoutAssembly_ReturnsCorrectType()
        {
            // Arrange
            // List<Dictionary<string, int>>
            string typeName = "System.Collections.Generic.List`1[[System.Collections.Generic.Dictionary`2[[System.String],[System.Int32]]]]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<Dictionary<string, int>>), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic Dictionary with all assembly information missing.
        /// </summary>
        [Test]
        public void FindType_GenericDictionaryAllMissingAssembly_ReturnsCorrectType()
        {
            // Arrange
            // Both main type and type arguments lack assembly info
            string typeName = "System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Int32]]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic type with namespace only.
        /// </summary>
        [Test]
        public void FindType_GenericTypeNamespaceOnly_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List`1[[System.Int32]]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic type only.
        /// </summary>
        [Test]
        public void FindType_GenericTypeOnly_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List`1";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<>), result);
        }

        /// <summary>
        /// Verifies that FindType handles mixed assembly information (some args have it, some don't).
        /// </summary>
        [Test]
        public void FindType_GenericTypeMixedAssemblyInfo_ReturnsCorrectType()
        {
            // Arrange
            // First arg has assembly info, second doesn't
            string typeName = "System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Int32]]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        #endregion

        #region FindType With Alias Tests

        /// <summary>
        /// Verifies that FindType resolves "int" alias to System.Int32.
        /// </summary>
        [Test]
        public void FindType_IntAlias_ReturnsInt32Type()
        {
            // Arrange
            string typeName = "int";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "string" alias to System.String.
        /// </summary>
        [Test]
        public void FindType_StringAlias_ReturnsStringType()
        {
            // Arrange
            string typeName = "string";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(string), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "bool" alias to System.Boolean.
        /// </summary>
        [Test]
        public void FindType_BoolAlias_ReturnsBooleanType()
        {
            // Arrange
            string typeName = "bool";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(bool), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "float" alias to System.Single.
        /// </summary>
        [Test]
        public void FindType_FloatAlias_ReturnsSingleType()
        {
            // Arrange
            string typeName = "float";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(float), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "double" alias to System.Double.
        /// </summary>
        [Test]
        public void FindType_DoubleAlias_ReturnsDoubleType()
        {
            // Arrange
            string typeName = "double";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(double), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "long" alias to System.Int64.
        /// </summary>
        [Test]
        public void FindType_LongAlias_ReturnsInt64Type()
        {
            // Arrange
            string typeName = "long";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(long), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "short" alias to System.Int16.
        /// </summary>
        [Test]
        public void FindType_ShortAlias_ReturnsInt16Type()
        {
            // Arrange
            string typeName = "short";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(short), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "byte" alias to System.Byte.
        /// </summary>
        [Test]
        public void FindType_ByteAlias_ReturnsByteType()
        {
            // Arrange
            string typeName = "byte";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(byte), result);
        }

        /// <summary>
        /// Verifies that FindType resolves "object" alias to System.Object.
        /// </summary>
        [Test]
        public void FindType_ObjectAlias_ReturnsObjectType()
        {
            // Arrange
            string typeName = "object";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(object), result);
        }

        /// <summary>
        /// Verifies that FindType returns null for unknown alias.
        /// </summary>
        [Test]
        public void FindType_UnknownAlias_ReturnsNull()
        {
            // Arrange
            string typeName = "unknownalias";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies that FindType is case-sensitive when resolving aliases.
        /// </summary>
        [Test]
        public void FindType_AliasCaseSensitive_ReturnsNull()
        {
            // Arrange
            string typeName = "INT"; // uppercase should not match "int"

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies that FindType caches alias resolution results.
        /// </summary>
        [Test]
        public void FindType_AliasCalledMultipleTimes_ReturnsSameResult()
        {
            // Arrange
            string typeName = "int";

            // Act
            var result1 = TypeUtility.FindType(typeName);
            var result2 = TypeUtility.FindType(typeName);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        #endregion

        #region FindType With Code-Like Generic Syntax Tests

        /// <summary>
        /// Verifies that FindType handles code-like generic List syntax.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericList_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<System.String>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<string>), result);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic Dictionary syntax.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericDictionary_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.Dictionary<System.String, System.Int32>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic type with built-in alias.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericWithAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<int>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles nested code-like generic types.
        /// </summary>
        [Test]
        public void FindType_CodeLikeNestedGeneric_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Int32>>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<Dictionary<string, int>>), result);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic type without namespace.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericWithoutNamespace_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "List<System.Int32>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic Tuple with three arguments.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericTuple_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Tuple<System.Int32, System.String, System.Boolean>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Tuple<int, string, bool>), result);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic type with mixed aliases and types.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericMixedAliases_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.Dictionary<string, int>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic Nullable type.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericNullable_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Nullable<System.Int32>";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int?), result);
        }

        /// <summary>
        /// Verifies that FindType caches code-like generic type resolution results.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericCalledMultipleTimes_ReturnsSameResult()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<System.String>";

            // Act
            var result1 = TypeUtility.FindType(typeName);
            var result2 = TypeUtility.FindType(typeName);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic type with whitespace.
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericWithWhitespace_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.Dictionary< System.String , System.Int32 >";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        #endregion

        #region FindType With Array Type Tests

        /// <summary>
        /// Verifies that FindType handles int[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_IntArrayAlias_ReturnsIntArrayType()
        {
            // Arrange
            string typeName = "int[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(int), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles string[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_StringArrayAlias_ReturnsStringArrayType()
        {
            // Arrange
            string typeName = "string[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(string[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(string), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles bool[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_BoolArrayAlias_ReturnsBoolArrayType()
        {
            // Arrange
            string typeName = "bool[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(bool[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(bool), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles float[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_FloatArrayAlias_ReturnsFloatArrayType()
        {
            // Arrange
            string typeName = "float[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(float[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(float), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles double[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_DoubleArrayAlias_ReturnsDoubleArrayType()
        {
            // Arrange
            string typeName = "double[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(double[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(double), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles long[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_LongArrayAlias_ReturnsLongArrayType()
        {
            // Arrange
            string typeName = "long[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(long[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(long), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles short[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_ShortArrayAlias_ReturnsShortArrayType()
        {
            // Arrange
            string typeName = "short[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(short[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(short), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles byte[] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_ByteArrayAlias_ReturnsByteArrayType()
        {
            // Arrange
            string typeName = "byte[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(byte[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(byte), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles two-dimensional int[,] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_Int2DArrayAlias_ReturnsInt2DArrayType()
        {
            // Arrange
            string typeName = "int[,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int[,]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(2, result.GetArrayRank());
        }

        /// <summary>
        /// Verifies that FindType handles three-dimensional int[,,] array type with alias element type.
        /// </summary>
        [Test]
        public void FindType_Int3DArrayAlias_ReturnsInt3DArrayType()
        {
            // Arrange
            string typeName = "int[,,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int[,,]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(3, result.GetArrayRank());
        }

        /// <summary>
        /// Verifies that FindType handles jagged array int[][] (array of int arrays).
        /// </summary>
        [Test]
        public void FindType_IntJaggedArrayAlias_ReturnsIntJaggedArrayType()
        {
            // Arrange
            string typeName = "int[][]";

            // Act
            var result = TypeUtility.FindType(typeName);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int[][]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(int[]), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles array of generic types with alias element type.
        /// </summary>
        [Test]
        public void FindType_GenericListIntArrayAlias_ReturnsGenericListIntArrayType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<int>[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<int>[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(List<int>), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles array of Dictionary with alias element types.
        /// </summary>
        [Test]
        public void FindType_GenericDictionaryStringIntArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.Dictionary<string,int>[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Dictionary<string, int>[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(Dictionary<string, int>), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles System.String[] array type (non-alias element type).
        /// </summary>
        [Test]
        public void FindType_SystemStringArray_ReturnsStringArrayType()
        {
            // Arrange
            string typeName = "System.String[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(string[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(string), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles System.Int32[] array type (non-alias element type).
        /// </summary>
        [Test]
        public void FindType_SystemInt32Array_ReturnsInt32ArrayType()
        {
            // Arrange
            string typeName = "System.Int32[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int[]), result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(int), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType caches array type resolution results.
        /// </summary>
        [Test]
        public void FindType_ArrayAliasCalledMultipleTimes_ReturnsSameResult()
        {
            // Arrange
            string typeName = "int[]";

            // Act
            var result1 = TypeUtility.FindType(typeName);
            var result2 = TypeUtility.FindType(typeName);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        /// <summary>
        /// Verifies that FindType returns null for array type with unknown element type.
        /// </summary>
        [Test]
        public void FindType_ArrayWithUnknownElementType_ReturnsNull()
        {
            // Arrange
            string typeName = "UnknownType[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies that FindType returns null for invalid array syntax (missing closing bracket).
        /// </summary>
        [Test]
        public void FindType_InvalidArraySyntax_ReturnsNull()
        {
            // Arrange
            string typeName = "int["; // Missing closing bracket

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies that FindType returns null for invalid array syntax (opening bracket at start).
        /// </summary>
        [Test]
        public void FindType_InvalidArraySyntaxBracketAtStart_ReturnsNull()
        {
            // Arrange
            string typeName = "[]int"; // Bracket at start is invalid

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies that FindType handles mixed multi-dimensional arrays (int[,][,,][]).
        /// </summary>
        [Test]
        public void FindType_IntMixedMultiDimensionalArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "int[,][,,][]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(int[,,][,]), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles complex mixed multi-dimensional arrays (int[,,,][][,]).
        /// </summary>
        [Test]
        public void FindType_IntComplexMixedMultiDimensionalArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "int[,,,][][,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
        }

        /// <summary>
        /// Verifies that FindType handles multi-level jagged arrays (int[][][]).
        /// </summary>
        [Test]
        public void FindType_IntMultiLevelJaggedArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "int[][][]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(int[][]), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles generic List two-dimensional array (List<int>[,]).
        /// </summary>
        [Test]
        public void FindType_GenericListInt2DArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<int>[,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(2, result.GetArrayRank());
            Assert.AreEqual(typeof(List<int>), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles generic List three-dimensional array (List<int>[,,]).
        /// </summary>
        [Test]
        public void FindType_GenericListInt3DArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<int>[,,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(3, result.GetArrayRank());
            Assert.AreEqual(typeof(List<int>), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles generic List with mixed multi-dimensional arrays (List<int>[][,,][]).
        /// </summary>
        [Test]
        public void FindType_GenericListIntMixedMultiDimensionalArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<int>[][,,][]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(List<int>[,,][]), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles Dictionary<string, int> with mixed multi-dimensional arrays.
        /// </summary>
        [Test]
        public void FindType_GenericDictionaryStringIntMixedMultiDimensionalArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.Dictionary<string,int>[,][,][]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
        }

        /// <summary>
        /// Verifies that FindType handles nested generic List<Dictionary<string, int>> with arrays.
        /// </summary>
        [Test]
        public void FindType_NestedGenericListDictionaryArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<System.Collections.Generic.Dictionary<string,int>>[,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(2, result.GetArrayRank());
        }

        /// <summary>
        /// Verifies that FindType handles complex nested generic with mixed multi-dimensional arrays.
        /// </summary>
        [Test]
        public void FindType_NestedGenericListDictionaryMixedMultiDimensionalArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<System.Collections.Generic.Dictionary<string,int>>[,][,][]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
        }

        /// <summary>
        /// Verifies that FindType handles generic List array with full namespace syntax.
        /// </summary>
        [Test]
        public void FindType_GenericListSystemInt32Array_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<System.Int32>[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(List<int>[]), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic Tuple array type.
        /// </summary>
        [Test]
        public void FindType_GenericTupleArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Tuple<int, string, bool>[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(Tuple<int, string, bool>[]), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic Nullable array type.
        /// </summary>
        [Test]
        public void FindType_GenericNullableArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Nullable<int>[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(int?[]), result);
        }

        /// <summary>
        /// Verifies that FindType handles generic Nullable with two-dimensional array.
        /// </summary>
        [Test]
        public void FindType_GenericNullable2DArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Nullable<int>[,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(2, result.GetArrayRank());
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic syntax with arrays (List<int>[]).
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericListIntArray_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.List<int>[]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(typeof(List<int>[]), result);
        }

        /// <summary>
        /// Verifies that FindType handles code-like generic syntax with two-dimensional arrays (Dictionary<string, int>[,]).
        /// </summary>
        [Test]
        public void FindType_CodeLikeGenericDictionaryStringInt2DArray_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "System.Collections.Generic.Dictionary<string, int>[,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
            Assert.AreEqual(2, result.GetArrayRank());
            Assert.AreEqual(typeof(Dictionary<string, int>), result.GetElementType());
        }

        /// <summary>
        /// Verifies that FindType handles complex string array with mixed dimensions (string[,][,][]).
        /// </summary>
        [Test]
        public void FindType_StringMixedMultiDimensionalArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "string[,][,][]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
        }

        /// <summary>
        /// Verifies that FindType handles bool array with mixed dimensions (bool[][][,]).
        /// </summary>
        [Test]
        public void FindType_BoolMixedMultiDimensionalArrayAlias_ReturnsCorrectType()
        {
            // Arrange
            string typeName = "bool[][][,]";

            // Act
            var result = TypeUtility.FindType(typeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsArray);
        }

        /// <summary>
        /// Verifies that FindType caches mixed multi-dimensional array type resolution results.
        /// </summary>
        [Test]
        public void FindType_MixedMultiDimensionalArrayCalledMultipleTimes_ReturnsSameResult()
        {
            // Arrange
            string typeName = "int[,][,,][]";

            // Act
            var result1 = TypeUtility.FindType(typeName);
            var result2 = TypeUtility.FindType(typeName);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        #endregion
    }
}
