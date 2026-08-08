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
    using System.Collections.Generic;

    internal class ResultContainer
    {
        private const string CanvasResponseKey = "response";

        public ResultContainer(IDictionary<string, object> dictionary)
        {
            this.RawResult = dictionary.ToJson();
            this.ResultDictionary = dictionary;
            if (Constants.IsWeb)
            {
                this.ResultDictionary = this.GetWebFormattedResponseDictionary(this.ResultDictionary);
            }
        }

        public ResultContainer(string result)
        {
            this.RawResult = result;

            if (!string.IsNullOrEmpty(result))
            {
                this.ResultDictionary = Facebook.MiniJSON.Json.Deserialize(result) as Dictionary<string, object>;

                if (Constants.IsWeb)
                {
                    // Web has a different format from mobile so reformat the result to match our
                    // mobile responses
                    this.ResultDictionary = this.GetWebFormattedResponseDictionary(this.ResultDictionary);
                }
            }
        }

        public string RawResult { get; private set; }

        public IDictionary<string, object> ResultDictionary { get; set; }

        private IDictionary<string, object> GetWebFormattedResponseDictionary(IDictionary<string, object> resultDictionary)
        {
            IDictionary<string, object> responseDictionary;
            if (resultDictionary.TryGetValue(CanvasResponseKey, out responseDictionary))
            {
                object callbackId;
                if (resultDictionary.TryGetValue(Constants.CallbackIdKey, out callbackId))
                {
                    responseDictionary[Constants.CallbackIdKey] = callbackId;
                }

                return responseDictionary;
            }

            return resultDictionary;
        }
    }
}
