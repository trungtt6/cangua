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

    internal class AppRequestResult : ResultBase, IAppRequestResult
    {
        public const string RequestIDKey = "request";
        public const string ToKey = "to";

        public AppRequestResult(ResultContainer resultContainer) : base(resultContainer)
        {
            if (this.ResultDictionary != null)
            {
                string requestID;
                if (this.ResultDictionary.TryGetValue(AppRequestResult.RequestIDKey, out requestID))
                {
                    this.RequestID = requestID;
                }

                string toStr;
                if (this.ResultDictionary.TryGetValue(AppRequestResult.ToKey, out toStr))
                {
                    this.To = toStr.Split(',');
                }
                else
                {
                    // On iOS the to field is an array of IDs
                    IEnumerable<object> toArray;
                    if (this.ResultDictionary.TryGetValue(AppRequestResult.ToKey, out toArray))
                    {
                        var toList = new List<string>();
                        foreach (object toEntry in toArray)
                        {
                            var toID = toEntry as string;
                            if (toID != null)
                            {
                                toList.Add(toID);
                            }
                        }

                        this.To = toList;
                    }
                }
            }
        }

        public string RequestID { get; private set; }

        public IEnumerable<string> To { get; private set; }

        public override string ToString()
        {
            return Utilities.FormatToString(
                base.ToString(),
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "RequestID", this.RequestID },
                    { "To", this.To != null ? this.To.ToCommaSeparateList() : null },
                });
        }
    }
}
