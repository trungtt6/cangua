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
    /// A class contiaing the result data.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error string from the result. If no error occured value is null or empty.</value>
        string Error { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>A collection of key values pairs that are parsed from the result.</value>
        IDictionary<string, object> ResultDictionary { get; }

        /// <summary>
        /// Gets the raw result.
        /// </summary>
        /// <value>The raw result string.</value>
        string RawResult { get; }

        /// <summary>
        /// Gets a value indicating whether this instance cancelled.
        /// </summary>
        /// <value><c>true</c> if this instance cancelled; otherwise, <c>false</c>.</value>b
        bool Cancelled { get; }
    }
}
