/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_5_6_OR_NEWER && !UNITY_5_6_0
using UnityEngine;
using UnityEngine.Store;

namespace AppStoresSupport
{
    [System.Serializable]
    public class AppStoreSetting 
    {
        public string AppID = "";
        public string AppKey = "";
        public bool IsTestMode = false;
    }

    [System.Serializable]
    public class AppStoreSettings : ScriptableObject
    {
        public string UnityClientID = "";
        public string UnityClientKey = "";
        public string UnityClientRSAPublicKey = "";

        public AppStoreSetting XiaomiAppStoreSetting = new AppStoreSetting();
        
        public AppInfo getAppInfo() {
            AppInfo appInfo = new AppInfo();
            appInfo.clientId = UnityClientID;
            appInfo.clientKey = UnityClientKey;
            appInfo.appId = XiaomiAppStoreSetting.AppID;
            appInfo.appKey = XiaomiAppStoreSetting.AppKey;
            appInfo.debug = XiaomiAppStoreSetting.IsTestMode;
            return appInfo;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/App Store Settings", false, 1011)]
        static void CreateAppStoreSettingsAsset()
        {
            const string appStoreSettingsAssetFolder = "Assets/Plugins/UnityChannel/XiaomiSupport/Resources";
            const string appStoreSettingsAssetPath = appStoreSettingsAssetFolder + "/AppStoreSettings.asset";
            if (File.Exists(appStoreSettingsAssetPath))
                return;

            if (!Directory.Exists(appStoreSettingsAssetFolder))
                Directory.CreateDirectory(appStoreSettingsAssetFolder);

            var appStoreSettings = CreateInstance<AppStoreSettings>();
            AssetDatabase.CreateAsset(appStoreSettings, appStoreSettingsAssetPath);
        }
#endif
    }
}
#endif
