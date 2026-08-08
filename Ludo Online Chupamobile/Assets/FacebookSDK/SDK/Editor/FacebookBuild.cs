/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Editor
{
    using System.Globalization;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    internal class FacebookBuild
    {
        private const string FacebookPath = "Assets/FacebookSDK/SDK/";
        private const string ExamplesPath = "Assets/FacebookSDK/Examples/";
        private const string PluginsPath = "Assets/FacebookSDK/Plugins/";

        public enum Target
        {
            DEBUG,
            RELEASE
        }

        private static string PackageName
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "facebook-unity-sdk-{0}.unitypackage",
                    FacebookSdkVersion.Build);
            }
        }

        private static string OutputPath
        {
            get
            {
                DirectoryInfo projectRoot = Directory.GetParent(Directory.GetCurrentDirectory());
                var outputDirectory = new DirectoryInfo(Path.Combine(projectRoot.FullName, "out"));

                // Create the directory if it doesn't exist
                outputDirectory.Create();
                return Path.Combine(outputDirectory.FullName, FacebookBuild.PackageName);
            }
        }

        // Exporting the *.unityPackage for Asset store
        public static string ExportPackage()
        {
            Debug.Log("Exporting Facebook Unity Package...");
            string path = OutputPath;
            try
            {
                if (!File.Exists(Path.Combine(Application.dataPath, "Temp")))
                {
                    AssetDatabase.CreateFolder("Assets", "Temp");
                }

                AssetDatabase.MoveAsset(FacebookPath + "Resources/FacebookSettings.asset", "Assets/Temp/FacebookSettings.asset");
                AssetDatabase.DeleteAsset(PluginsPath + "Android/AndroidManifest.xml");
                AssetDatabase.DeleteAsset(PluginsPath + "Android/AndroidManifest.xml.meta");

                string[] facebookFiles = (string[])Directory.GetFiles(FacebookPath, "*.*", SearchOption.AllDirectories);
                string[] exampleFiles = (string[])Directory.GetFiles(ExamplesPath, "*.*", SearchOption.AllDirectories);
                string[] pluginsFiles = (string[])Directory.GetFiles(PluginsPath, "*.*", SearchOption.AllDirectories);
                string[] files = new string[facebookFiles.Length + exampleFiles.Length + pluginsFiles.Length];
                facebookFiles.CopyTo(files, 0);
                exampleFiles.CopyTo(files, facebookFiles.Length);
                pluginsFiles.CopyTo(files, facebookFiles.Length + exampleFiles.Length);

                AssetDatabase.ExportPackage(
                    files,
                    path,
                    ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
            }
            finally
            {
                // Move files back no matter what
                AssetDatabase.MoveAsset("Assets/Temp/FacebookSettings.asset", FacebookPath + "Resources/FacebookSettings.asset");
                AssetDatabase.DeleteAsset("Assets/Temp");

                // regenerate the manifest
                UnityEditor.FacebookEditor.ManifestMod.GenerateManifest();
            }

            Debug.Log("Finished exporting!");

            return path;
        }
    }
}
