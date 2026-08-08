/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// #define DISABLE_IDFA // If you need to disable IDFA for your game, uncomment this

using System;
using System.Runtime.InteropServices;

namespace PlayFab
{
    public static class PlayFabiOSPlugin
    {

#if UNITY_IOS && !DISABLE_IDFA
        [DllImport("__Internal")]
        public static extern string getIdfa();
        [DllImport("__Internal")]
        public static extern bool getAdvertisingDisabled();
#elif UNITY_IOS
        public static string getIdfa() { return "invalid"; }

        public static bool getAdvertisingDisabled() { return true; }
#endif
        
    }
}
