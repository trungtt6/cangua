/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity
{
    using Facebook.Unity.Mobile.Android;
    using UnityEngine;

    internal static class FacebookLogger
    {
        private const string UnityAndroidTag = "Facebook.Unity.FBDebug";

        static FacebookLogger()
        {
            FacebookLogger.Instance = new CustomLogger();
        }

        internal static IFacebookLogger Instance { private get; set; }

        public static void Log(string msg)
        {
            FacebookLogger.Instance.Log(msg);
        }

        public static void Log(string format, params string[] args)
        {
            FacebookLogger.Log(string.Format(format, args));
        }

        public static void Info(string msg)
        {
            FacebookLogger.Instance.Info(msg);
        }

        public static void Info(string format, params string[] args)
        {
            FacebookLogger.Info(string.Format(format, args));
        }

        public static void Warn(string msg)
        {
            FacebookLogger.Instance.Warn(msg);
        }

        public static void Warn(string format, params string[] args)
        {
            FacebookLogger.Warn(string.Format(format, args));
        }

        public static void Error(string msg)
        {
            FacebookLogger.Instance.Error(msg);
        }

        public static void Error(string format, params string[] args)
        {
            FacebookLogger.Error(string.Format(format, args));
        }

        private class CustomLogger : IFacebookLogger
        {
            private IFacebookLogger logger;

            public CustomLogger()
            {
#if UNITY_EDITOR
                this.logger = new EditorLogger();
#elif UNITY_ANDROID
                this.logger = new AndroidLogger();
#elif UNITY_IOS
                this.logger = new IOSLogger();
#else
                this.logger = new CanvasLogger();
#endif
            }

            public void Log(string msg)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log(msg);
                    this.logger.Log(msg);
                }
            }

            public void Info(string msg)
            {
                Debug.Log(msg);
                this.logger.Info(msg);
            }

            public void Warn(string msg)
            {
                Debug.LogWarning(msg);
                this.logger.Warn(msg);
            }

            public void Error(string msg)
            {
                Debug.LogError(msg);
                this.logger.Error(msg);
            }
        }

#if UNITY_EDITOR
        private class EditorLogger : IFacebookLogger
        {
            public void Log(string msg)
            {
            }

            public void Info(string msg)
            {
            }

            public void Warn(string msg)
            {
            }

            public void Error(string msg)
            {
            }
        }

#elif UNITY_ANDROID
        private class AndroidLogger : IFacebookLogger
        {
            public void Log(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("v", UnityAndroidTag, msg);
                }
            }

            public void Info(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("i", UnityAndroidTag, msg);
                }
            }

            public void Warn(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("w", UnityAndroidTag, msg);
                }
            }

            public void Error(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("e", UnityAndroidTag, msg);
                }
            }
        }
#elif UNITY_IOS
        private class IOSLogger: IFacebookLogger
        {
            public void Log(string msg)
            {
                // TODO
            }

            public void Info(string msg)
            {
                // TODO
            }

            public void Warn(string msg)
            {
                // TODO
            }

            public void Error(string msg)
            {
                // TODO
            }
        }
#else
        private class CanvasLogger : IFacebookLogger
        {
            public void Log(string msg)
            {
                Application.ExternalCall("console.log", msg);
            }

            public void Info(string msg)
            {
                Application.ExternalCall("console.info", msg);
            }

            public void Warn(string msg)
            {
                Application.ExternalCall("console.warn", msg);
            }

            public void Error(string msg)
            {
                Application.ExternalCall("console.error", msg);
            }
        }
#endif
    }
}
