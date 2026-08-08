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
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using AudienceNetwork;

    public class AudienceNetworkSettingsEditor : UnityEditor.Editor
    {
        private static string title = "Audience Network SDK";

        [MenuItem("Tools/Audience Network/About")]
        private static void AboutGUI ()
        {
            string aboutString = System.String.Format ("Facebook Audience Network Unity SDK Version {0}",
                                               AudienceNetwork.SdkVersion.Build);
            EditorUtility.DisplayDialog (title,
                                         aboutString,
                                         "Okay");
        }

        [MenuItem("Tools/Audience Network/Regenerate Android Manifest")]
        private static void RegenerateManifest ()
        {
            bool updateManifest = EditorUtility.DisplayDialog (title,
                                                               "Are you sure you want to regenerate your Android Manifest.xml?",
                                                               "Okay",
                                                               "Cancel");

            if (updateManifest) {
                AudienceNetwork.Editor.ManifestMod.GenerateManifest ();
                EditorUtility.DisplayDialog (title, "Android Manifest updated. \n \n If interstitial ads still throw ActivityNotFoundException, " +
                    "you may need to copy the generated manifest at " + ManifestMod.AndroidManifestPath + " to /Assets/Plugins/Android.", "Okay");
            }
        }

        [MenuItem("Tools/Audience Network/Build SDK Package")]
        private static void BuildGUI ()
        {
            try {
                string exportedPath = AudienceNetworkBuild.ExportPackage ();
                EditorUtility.DisplayDialog (title, "Exported to " + exportedPath, "Okay");

            } catch (System.Exception e) {
                EditorUtility.DisplayDialog (title, e.Message, "Okay");
            }
        }
    }
}
