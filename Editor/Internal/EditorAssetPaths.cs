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
            TemporaryDirectory = EasyPath.FromPath(Path.GetTempPath())
                .AddName(PlayerSettings.companyName)
                .AddName(PlayerSettings.productName)
                .AddName("EasyToolKit")
                .ToString();
        }
    }
}
