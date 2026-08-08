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
    using System.Collections.Generic;

    /// <summary>
    /// A result containing an app link.
    /// </summary>
    public interface IAppLinkResult : IResult
    {
        /// <summary>
        /// Gets the URL. This is the url that was used to open the app on iOS
        /// or on Android the intent's data string. When handling deffered app
        /// links on Android this may not be available.
        /// </summary>
        /// <value>The link url.</value>
        string Url { get; }

        /// <summary>
        /// Gets the target URI.
        /// </summary>
        /// <value>The target uri for this App Link.</value>
        string TargetUrl { get; }

        /// <summary>
        /// Gets the ref.
        /// </summary>
        /// <value> Returns the ref for this App Link.
        /// The referer data associated with the app link.
        /// This will contain Facebook specific information like fb_access_token, fb_expires_in, and fb_ref.
        /// </value>
        string Ref { get; }

        /// <summary>
        /// Gets the extras.
        /// </summary>
        /// <value>
        /// The full set of arguments for this app link. Properties like target uri &amp; ref are typically
        /// picked out of this set of arguments.
        /// </value>
        IDictionary<string, object> Extras { get; }
    }
}
