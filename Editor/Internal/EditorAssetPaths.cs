using System.IO;
using EasyToolKit.Core.IO;
using UnityEditor;

namespace EasyToolKit.Core.Editor.Internal
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
                "EasyToolKit");
        }
    }
}
