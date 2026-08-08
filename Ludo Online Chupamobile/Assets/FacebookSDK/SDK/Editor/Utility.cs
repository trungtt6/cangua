/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Utility
    {
        public static T Pop<T>(this IList<T> list)
        {
            if (!list.Any())
            {
                throw new InvalidOperationException("Attempting to pop item on empty list.");
            }

            int index = list.Count - 1;
            T value = list[index];
            list.RemoveAt(index);
            return value;
        }

        public static bool TryGetValue<T>(
            this IDictionary<string, object> dictionary,
            string key,
            out T value)
        {
            object resultObj;
            if (dictionary.TryGetValue(key, out resultObj) && resultObj is T)
            {
                value = (T)resultObj;
                return true;
            }

            value = default(T);
            return false;
        }
    }
}
