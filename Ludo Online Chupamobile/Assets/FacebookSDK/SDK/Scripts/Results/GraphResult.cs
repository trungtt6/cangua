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

    internal class GraphResult : ResultBase, IGraphResult
    {
        internal GraphResult(WWW result) : base(new ResultContainer(result.text), result.error, false)
        {
            this.Init(this.RawResult);

            // The WWW object will throw an exception if accessing the texture field and
            // an error has occured.
            if (result.error == null)
            {
                // The Graph API does not return textures directly, but a few endpoints can
                // redirect to images when no 'redirect=false' parameter is specified. Ex: '/me/picture'
                this.Texture = result.texture;
            }
        }

        public IList<object> ResultList { get; private set; }

        public Texture2D Texture { get; private set; }

        private void Init(string rawResult)
        {
            if (string.IsNullOrEmpty(rawResult))
            {
                return;
            }

            object serailizedResult = MiniJSON.Json.Deserialize(this.RawResult);
            var jsonObject = serailizedResult as IDictionary<string, object>;
            if (jsonObject != null)
            {
                this.ResultDictionary = jsonObject;
                return;
            }

            var jsonArray = serailizedResult as IList<object>;
            if (jsonArray != null)
            {
                this.ResultList = jsonArray;
                return;
            }
        }
    }
}
