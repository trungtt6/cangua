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

    internal class MethodArguments
    {
        private IDictionary<string, object> arguments = new Dictionary<string, object>();

        public MethodArguments() : this(new Dictionary<string, object>())
        {
        }

        public MethodArguments(MethodArguments methodArgs) : this(methodArgs.arguments)
        {
        }

        private MethodArguments(IDictionary<string, object> arguments)
        {
            this.arguments = arguments;
        }

        public void AddPrimative<T>(string argumentName, T value) where T : struct
        {
            this.arguments[argumentName] = value;
        }

        public void AddNullablePrimitive<T>(string argumentName, T? nullable) where T : struct
        {
            if (nullable != null && nullable.HasValue)
            {
                this.arguments[argumentName] = nullable.Value;
            }
        }

        public void AddString(string argumentName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.arguments[argumentName] = value;
            }
        }

        public void AddCommaSeparatedList(string argumentName, IEnumerable<string> value)
        {
            if (value != null)
            {
                this.arguments[argumentName] = value.ToCommaSeparateList();
            }
        }

        public void AddDictionary(string argumentName, IDictionary<string, object> dict)
        {
            if (dict != null)
            {
                this.arguments[argumentName] = MethodArguments.ToStringDict(dict);
            }
        }

        public void AddList<T>(string argumentName, IEnumerable<T> list)
        {
            if (list != null)
            {
                this.arguments[argumentName] = list;
            }
        }

        public void AddUri(string argumentName, Uri uri)
        {
            if (uri != null && !string.IsNullOrEmpty(uri.AbsoluteUri))
            {
                this.arguments[argumentName] = uri.ToString();
            }
        }

        public string ToJsonString()
        {
            return MiniJSON.Json.Serialize(this.arguments);
        }

        private static Dictionary<string, string> ToStringDict(IDictionary<string, object> dict)
        {
            if (dict == null)
            {
                return null;
            }

            var newDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                newDict[kvp.Key] = kvp.Value.ToString();
            }

            return newDict;
        }
    }
}
