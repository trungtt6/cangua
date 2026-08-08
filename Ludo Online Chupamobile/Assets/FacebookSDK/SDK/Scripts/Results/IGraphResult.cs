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
    using UnityEngine;

    /// <summary>
    /// The result of a graph api call.
    /// </summary>
    public interface IGraphResult : IResult
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>A list parsed from the result.</value>
        IList<object> ResultList { get; }

        /// <summary>
        /// Gets the Texture.
        /// </summary>
        /// <value>
        /// A texture downloaded from the graph endpoint if the graph api redirected to a image, otherwise null.
        /// </value>
        /// <remarks>
        ///     The Graph API does not return textures directly, but a few endpoints can
        ///     redirect to images when no 'redirect=false' parameter is specified. Ex: '/me/picture'.
        /// </remarks>
        Texture2D Texture { get; }
    }
}
