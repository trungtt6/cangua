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

    internal class AppLinkResult : ResultBase, IAppLinkResult
    {
        public AppLinkResult(ResultContainer resultContainer) : base(resultContainer)
        {
            if (this.ResultDictionary != null)
            {
                string url;
                if (this.ResultDictionary.TryGetValue<string>(Constants.UrlKey, out url))
                {
                    this.Url = url;
                }

                string targetUrl;
                if (this.ResultDictionary.TryGetValue<string>(Constants.TargetUrlKey, out targetUrl))
                {
                    this.TargetUrl = targetUrl;
                }

                string refStr;
                if (this.ResultDictionary.TryGetValue<string>(Constants.RefKey, out refStr))
                {
                    this.Ref = refStr;
                }

                IDictionary<string, object> argumentBundle;
                if (this.ResultDictionary.TryGetValue<IDictionary<string, object>>(Constants.ExtrasKey, out argumentBundle))
                {
                    this.Extras = argumentBundle;
                }
            }
        }

        public string Url { get; private set; }

        public string TargetUrl { get; private set; }

        public string Ref { get; private set; }

        public IDictionary<string, object> Extras { get; private set; }

        public override string ToString()
        {
            return Utilities.FormatToString(
                base.ToString(),
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "Url", this.Url },
                    { "TargetUrl", this.TargetUrl },
                    { "Ref", this.Ref },
                    { "Extras", this.Extras.ToJson() },
                });
        }
    }
}
