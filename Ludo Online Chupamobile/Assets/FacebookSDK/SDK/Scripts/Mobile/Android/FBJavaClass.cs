/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Mobile.Android
{
    using UnityEngine;

    internal class FBJavaClass : IAndroidJavaClass
    {
        private const string FacebookJavaClassName = "com.facebook.unity.FB";
        private AndroidJavaClass facebookJavaClass = new AndroidJavaClass(FacebookJavaClassName);

        public T CallStatic<T>(string methodName)
        {
            return this.facebookJavaClass.CallStatic<T>(methodName);
        }

        public void CallStatic(string methodName, params object[] args)
        {
            this.facebookJavaClass.CallStatic(methodName, args);
        }

        // Mock the AndroidJava to compile on other platforms
        #if !UNITY_ANDROID
        private class AndroidJNIHelper
        {
            public static bool Debug { get; set; }
        }

        private class AndroidJavaClass
        {
            public AndroidJavaClass(string mock)
            {
            }

            public T CallStatic<T>(string method)
            {
                return default(T);
            }

            public void CallStatic(string method, params object[] args)
            {
            }
        }
        #endif
    }
}
