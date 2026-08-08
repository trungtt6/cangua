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
    /// App request result.
    /// </summary>
    public interface IAppRequestResult : IResult
    {
        /// <summary>
        /// Gets RequestID.
        /// </summary>
        /// <value>A request ID assigned by Facebook.</value>
        string RequestID { get; }

        /// <summary>
        /// Gets the list of users who the request was sent to.
        /// </summary>
        /// <value>An array of string, each element being the Facebook ID of one of the selected recipients.</value>
        IEnumerable<string> To { get; }
    }
}
