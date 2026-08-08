/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace UnityEditor.FacebookEditor
{
    using System.IO;
    using Facebook.Unity;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEngine;

    public static class XCodePostProcess
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            // If integrating with facebook on any platform, throw a warning if the app id is invalid
            if (!Facebook.Unity.FacebookSettings.IsValidAppId)
            {
                Debug.LogWarning("You didn't specify a Facebook app ID.  Please add one using the Facebook menu in the main Unity editor.");
            }

            // Unity renamed build target from iPhone to iOS in Unity 5, this keeps both versions happy
            if (target.ToString() == "iOS" || target.ToString() == "iPhone")
            {
                UpdatePlist(path);
                FixupFiles.FixSimulator(path);
                FixupFiles.AddVersionDefine(path);
                FixupFiles.FixColdStart(path);
            }

            if (target == BuildTarget.Android)
            {
                // The default Bundle Identifier for Unity does magical things that causes bad stuff to happen
                if (PlayerSettings.applicationIdentifier == "com.Company.ProductName")
                {
                    Debug.LogError("The default Unity Bundle Identifier (com.Company.ProductName) will not work correctly.");
                }

                if (!FacebookAndroidUtil.SetupProperly)
                {
                    Debug.LogError("Your Android setup is not correct. See Settings in Facebook menu.");
                }

                if (!ManifestMod.CheckManifest())
                {
                    // If something is wrong with the Android Manifest, try to regenerate it to fix it for the next build.
                    ManifestMod.GenerateManifest();
                }
            }
        }

        public static void UpdatePlist(string path)
        {
            const string FileName = "Info.plist";
            string appId = FacebookSettings.AppId;
            string fullPath = Path.Combine(path, FileName);

            if (string.IsNullOrEmpty(appId) || appId.Equals("0"))
            {
                Debug.LogError("You didn't specify a Facebook app ID.  Please add one using the Facebook menu in the main Unity editor.");
                return;
            }

            var facebookParser = new PListParser(fullPath);
            facebookParser.UpdateFBSettings(
                appId,
                FacebookSettings.IosURLSuffix,
                FacebookSettings.AppLinkSchemes[FacebookSettings.SelectedAppIndex].Schemes);
            facebookParser.WriteToFile();
        }
    }
}
