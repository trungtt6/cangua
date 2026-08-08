/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Diagnostics;

namespace AudienceNetwork.Editor
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEngine;
    using AudienceNetwork.Editor;

    public static class XCodePostProcess
    {
        public static string AudienceNetworkFramework = "FBAudienceNetwork.framework";
        public static string AudienceNetworkAAR = "ads-release.aar";
        public static string FrameworkDependenciesKey = "FrameworkDependencies";
        public static string RequiredFrameworks = "AdSupport;StoreKit;";

        [PostProcessBuild(100)]
        public static void OnPostProcessBuild (BuildTarget target, string path)
        {
            if (target == BuildTarget.Android) {
                // The default Bundle Identifier for Unity does magical things that causes bad stuff to happen
                if (PlayerSettings.applicationIdentifier == "com.Company.ProductName") {
                    Debug.LogError ("The default Unity Bundle Identifier (com.Company.ProductName) will not work correctly.");
                }

                if (!ManifestMod.CheckManifest())
                {
                    // If something is wrong with the Android Manifest, try to regenerate it to fix it for the next build.
                    ManifestMod.GenerateManifest();
                }
            } else if (target == BuildTarget.iOS) {
                ConfigurePluginPlatforms ();
            }
        }

        public static void ConfigurePluginPlatforms ()
        {
            PluginImporter[] importers = PluginImporter.GetAllImporters();
            PluginImporter iOSPlugin = null;
            PluginImporter androidPlugin = null;
            foreach (PluginImporter importer in importers) {
                if (importer.assetPath.Contains(AudienceNetworkFramework)) {
                    iOSPlugin = importer;
                    Debug.Log ("Audience Network iOS plugin found at " + importer.assetPath + ".");
                } else if (importer.assetPath.Contains(AudienceNetworkAAR)) {
                    androidPlugin = importer;
                    Debug.Log ("Audience Network Android plugin found at " + importer.assetPath + ".");
                }
            }
            if (iOSPlugin != null) {
                iOSPlugin.SetCompatibleWithAnyPlatform(false);
                iOSPlugin.SetCompatibleWithEditor(false);
                iOSPlugin.SetCompatibleWithPlatform(BuildTarget.iOS, true);
                iOSPlugin.SetPlatformData(BuildTarget.iOS, FrameworkDependenciesKey, RequiredFrameworks);
                iOSPlugin.SaveAndReimport();
            }
            if (androidPlugin != null) {
                androidPlugin.SetCompatibleWithAnyPlatform(false);
                androidPlugin.SetCompatibleWithEditor(false);
                androidPlugin.SetCompatibleWithPlatform(BuildTarget.Android, true);
                androidPlugin.SaveAndReimport();
            }
        }
    }
}
