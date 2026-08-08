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
    /// <summary>
    /// Share dialog mode.
    /// </summary>
    public enum ShareDialogMode
    {
        // If you make changes in here make the same changes in Assets/Facebook/Editor/iOS/FBUnityInterface.h

        /// <summary>
        /// The sdk will choose which type of dialog to show
        /// See the Facebook SDKs for ios and android for specific details.
        /// </summary>
        AUTOMATIC = 0,

        /// <summary>
        /// Uses the dialog inside the native facebook applications. Note this will fail if the
        /// native applications are not installed.
        /// </summary>
        NATIVE = 1,

        /// <summary>
        /// Opens the facebook dialog in a webview.
        /// </summary>
        WEB = 2,

        /// <summary>
        /// Uses the feed dialog.
        /// </summary>
        FEED = 3,
    }
}
