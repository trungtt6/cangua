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
    using System;
    using System.Globalization;
    using UnityEngine;

    internal static class Constants
    {
        // Callback keys
        public const string CallbackIdKey = "callback_id";
        public const string AccessTokenKey = "access_token";
        public const string UrlKey = "url";
        public const string RefKey = "ref";
        public const string ExtrasKey = "extras";
        public const string TargetUrlKey = "target_url";
        public const string CancelledKey = "cancelled";
        public const string ErrorKey = "error";

        // Callback Method Names
        public const string OnPayCompleteMethodName = "OnPayComplete";
        public const string OnShareCompleteMethodName = "OnShareLinkComplete";
        public const string OnAppRequestsCompleteMethodName = "OnAppRequestsComplete";
        public const string OnGroupCreateCompleteMethodName = "OnGroupCreateComplete";
        public const string OnGroupJoinCompleteMethodName = "OnJoinGroupComplete";

        // Graph API
        public const string GraphApiVersion = "v2.5";
        public const string GraphUrlFormat = "https://graph.{0}/{1}/";

        // Permission Strings
        public const string UserLikesPermission = "user_likes";
        public const string EmailPermission = "email";
        public const string PublishActionsPermission = "publish_actions";
        public const string PublishPagesPermission = "publish_pages";

        // The current platform. We save this in a variable to allow for
        // mocking during testing
        private static FacebookUnityPlatform? currentPlatform;

        /// <summary>
        /// Gets the graph URL.
        /// </summary>
        /// <value>The graph URL. Ex. https://graph.facebook.com/v2.5/.</value>
        public static Uri GraphUrl
        {
            get
            {
                string urlStr = string.Format(
                    CultureInfo.InvariantCulture,
                    Constants.GraphUrlFormat,
                    FB.FacebookDomain,
                    FB.GraphApiVersion);
                return new Uri(urlStr);
            }
        }

        public static string GraphApiUserAgent
        {
            get
            {
                // Return the Unity SDK User Agent and our platform user agent
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    FB.FacebookImpl.SDKUserAgent,
                    Constants.UnitySDKUserAgent);
            }
        }

        public static bool IsMobile
        {
            get
            {
                return Constants.CurrentPlatform == FacebookUnityPlatform.Android ||
                    Constants.CurrentPlatform == FacebookUnityPlatform.IOS;
            }
        }

        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsWeb
        {
            get
            {
                return Constants.CurrentPlatform == FacebookUnityPlatform.WebGL ||
                    Constants.CurrentPlatform == FacebookUnityPlatform.WebPlayer;
            }
        }

        /// <summary>
        /// Gets the legacy user agent suffix that gets
        /// appended to graph requests on ios and android.
        /// </summary>
        /// <value>The user agent unity suffix legacy.</value>
        public static string UnitySDKUserAgentSuffixLegacy
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "Unity.{0}",
                    FacebookSdkVersion.Build);
            }
        }

        /// <summary>
        /// Gets the Unity SDK user agent.
        /// </summary>
        public static string UnitySDKUserAgent
        {
            get
            {
                return Utilities.GetUserAgent("FBUnitySDK", FacebookSdkVersion.Build);
            }
        }

        public static bool DebugMode
        {
            get
            {
                return Debug.isDebugBuild;
            }
        }

        public static FacebookUnityPlatform CurrentPlatform
        {
            get
            {
                if (!Constants.currentPlatform.HasValue)
                {
                    Constants.currentPlatform = Constants.GetCurrentPlatform();
                }

                return Constants.currentPlatform.Value;
            }

            set
            {
                Constants.currentPlatform = value;
            }
        }

        private static FacebookUnityPlatform GetCurrentPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return FacebookUnityPlatform.Android;
                case RuntimePlatform.IPhonePlayer:
                    return FacebookUnityPlatform.IOS;
                case RuntimePlatform.WindowsWebPlayer:
                case RuntimePlatform.OSXWebPlayer:
                    return FacebookUnityPlatform.WebPlayer;
#if UNITY_5
                case RuntimePlatform.WebGLPlayer:
                    return FacebookUnityPlatform.WebGL;
#endif
                default:
                    return FacebookUnityPlatform.Unknown;
            }
        }
    }
}
