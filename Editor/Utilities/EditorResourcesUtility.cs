using System;
using System.Collections.Generic;
using System.IO;
using EasyToolkit.Core.Textual;
using UnityEditor;

namespace EasyToolkit.Core.Editor
{
    public static class EditorResourcesUtility
    {
        private static readonly Dictionary<string, string> PackageRelativePathByModuleName =
            new Dictionary<string, string>();

        public static T Load<T>(string moduleName, string assetName)
            where T : UnityEngine.Object
        {
            var packageRelativePath = GetPackageRelativePath(moduleName);
            var assetPath = $"{packageRelativePath}/Editor/Resources/{assetName}";
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
            {
                throw new ArgumentException();
            }

            return asset;
        }

        private static string GetPackageRelativePath(string moduleName)
        {
            if (PackageRelativePathByModuleName.TryGetValue(moduleName, out var packageRelativePath))
            {
                return packageRelativePath;
            }

            var packageName = $"com.easytoolkit.{moduleName}";
            var packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                if (Directory.Exists($"{packagePath}/Assets/Packages/{packageName}"))
                {
                    packageRelativePath = $"Assets/Packages/{packageName}";
                }
            }

            if(packageRelativePath.IsNullOrEmpty())
            {
                packagePath = Path.GetFullPath($"Packages/{packageName}");
                if (Directory.Exists(packagePath))
                {
                    packageRelativePath = $"Packages/{packageName}";
                }
            }

            if (packageRelativePath.IsNullOrEmpty())
            {
                throw new ArgumentException();
            }

            PackageRelativePathByModuleName.Add(moduleName, packageRelativePath);
            return packageRelativePath;
        }
    }
}
