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

    internal class CallbackManager
    {
        private IDictionary<string, object> facebookDelegates = new Dictionary<string, object>();
        private int nextAsyncId;

        public string AddFacebookDelegate<T>(FacebookDelegate<T> callback) where T : IResult
        {
            if (callback == null)
            {
                return null;
            }

            this.nextAsyncId++;
            this.facebookDelegates.Add(this.nextAsyncId.ToString(), callback);
            return this.nextAsyncId.ToString();
        }

        public void OnFacebookResponse(IInternalResult result)
        {
            if (result == null || result.CallbackId == null)
            {
                return;
            }

            object callback;
            if (this.facebookDelegates.TryGetValue(result.CallbackId, out callback))
            {
                CallCallback(callback, result);
                this.facebookDelegates.Remove(result.CallbackId);
            }
        }

        // Since unity mono doesn't support covariance and contravariance use this hack
        private static void CallCallback(object callback, IResult result)
        {
            if (callback == null || result == null)
            {
                return;
            }

            if (CallbackManager.TryCallCallback<IAppRequestResult>(callback, result) ||
                CallbackManager.TryCallCallback<IShareResult>(callback, result) ||
                CallbackManager.TryCallCallback<IGroupCreateResult>(callback, result) ||
                CallbackManager.TryCallCallback<IGroupJoinResult>(callback, result) ||
                CallbackManager.TryCallCallback<IPayResult>(callback, result) ||
                CallbackManager.TryCallCallback<IAppInviteResult>(callback, result) ||
                CallbackManager.TryCallCallback<IAppLinkResult>(callback, result) ||
                CallbackManager.TryCallCallback<ILoginResult>(callback, result) ||
                CallbackManager.TryCallCallback<IAccessTokenRefreshResult>(callback, result))
            {
                return;
            }

            throw new NotSupportedException("Unexpected result type: " + callback.GetType().FullName);
        }

        private static bool TryCallCallback<T>(object callback, IResult result) where T : IResult
        {
            var castedCallback = callback as FacebookDelegate<T>;
            if (castedCallback != null)
            {
                castedCallback((T)result);
                return true;
            }

            return false;
        }
    }
}
