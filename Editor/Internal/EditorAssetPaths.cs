using System.IO;
using EasyToolkit.Core.IO;
using UnityEditor;

namespace EasyToolkit.Core.Editor.Internal
{
    public static class EditorAssetPaths
    {
        public static readonly string TemporaryDirectory;

        public static string GetModuleTemporaryDirectory(string moduleName)
        {
            return $"{TemporaryDirectory}/{moduleName}";
        }

        static EditorAssetPaths()
        {
            TemporaryDirectory = Path.Combine(
                Path.GetTempPath(),
                PlayerSettings.companyName,
                PlayerSettings.productName,
                "EasyToolkit");
        }
    }
}
