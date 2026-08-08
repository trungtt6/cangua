/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace AudienceNetwork.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class AudienceNetworkBuild
    {
        public const string AudienceNetworkPath = "Assets/AudienceNetwork/";
        public const string AudienceNetworkPluginsPath = AudienceNetworkPath + "Plugins/";
        public static string PluginsPath = "Assets/Plugins/";

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
                    "audience-network-unity-sdk-{0}.unitypackage",
                    SdkVersion.Build);
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
                return Path.Combine(outputDirectory.FullName, AudienceNetworkBuild.PackageName);
            }
        }

        // Exporting the *.unityPackage for Asset store
        public static string ExportPackage ()
        {
            Debug.Log ("Exporting Audience Network Unity Package...");

            var path = AudienceNetworkBuild.OutputPath;

            try {
                AssetDatabase.DeleteAsset (PluginsPath + "Android/AndroidManifest.xml");
                AssetDatabase.DeleteAsset (PluginsPath + "Android/AndroidManifest.xml.meta");
                AssetDatabase.DeleteAsset (AudienceNetworkPluginsPath + "Android/AndroidManifest.xml");
                AssetDatabase.DeleteAsset (AudienceNetworkPluginsPath + "Android/AndroidManifest.xml.meta");

                string[] facebookFiles = (string[])Directory.GetFiles (AudienceNetworkPath, "*.*", SearchOption.AllDirectories);
                string[] pluginsFiles = (string[])Directory.GetFiles (AudienceNetworkPluginsPath, "*.*", SearchOption.AllDirectories);
                string[] files = new string[facebookFiles.Length + pluginsFiles.Length];

                facebookFiles.CopyTo (files, 0);
                pluginsFiles.CopyTo (files, facebookFiles.Length);

                AssetDatabase.ExportPackage (
                    files,
                    path,
                    ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
            } finally {
                // regenerate the manifest
                AudienceNetwork.Editor.ManifestMod.GenerateManifest ();
            }
            Debug.Log ("Finished exporting!");
            return path;
        }
    }
}
