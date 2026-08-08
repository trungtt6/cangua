/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Mobile
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Classes defined on the mobile sdks.
    /// </summary>
    internal abstract class MobileFacebook : FacebookBase, IMobileFacebookImplementation
    {
        private const string CallbackIdKey = "callback_id";
        private ShareDialogMode shareDialogMode = ShareDialogMode.AUTOMATIC;

        protected MobileFacebook(CallbackManager callbackManager) : base(callbackManager)
        {
        }

        /// <summary>
        /// Gets or sets the dialog mode.
        /// </summary>
        /// <value>The dialog mode use for sharing, login, and other dialogs.</value>
        public ShareDialogMode ShareDialogMode
        {
            get
            {
                return this.shareDialogMode;
            }

            set
            {
                this.shareDialogMode = value;
                this.SetShareDialogMode(this.shareDialogMode);
            }
        }

        public abstract void AppInvite(
            Uri appLinkUrl,
            Uri previewImageUrl,
            FacebookDelegate<IAppInviteResult> callback);

        public abstract void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback);

        public abstract void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback);

        public override void OnLoginComplete(ResultContainer resultContainer)
        {
            var result = new LoginResult(resultContainer);
            this.OnAuthResponse(result);
        }

        public override void OnGetAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupCreateComplete(ResultContainer resultContainer)
        {
            var result = new GroupCreateResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupJoinComplete(ResultContainer resultContainer)
        {
            var result = new GroupJoinResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnAppRequestsComplete(ResultContainer resultContainer)
        {
            var result = new AppRequestResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnAppInviteComplete(ResultContainer resultContainer)
        {
            var result = new AppInviteResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnFetchDeferredAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(ResultContainer resultContainer)
        {
            var result = new ShareResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnRefreshCurrentAccessTokenComplete(ResultContainer resultContainer)
        {
            var result = new AccessTokenRefreshResult(resultContainer);
            if (result.AccessToken != null)
            {
                AccessToken.CurrentAccessToken = result.AccessToken;
            }

            CallbackManager.OnFacebookResponse(result);
        }

        protected abstract void SetShareDialogMode(ShareDialogMode mode);

        private static IDictionary<string, object> DeserializeMessage(string message)
        {
            return (Dictionary<string, object>)MiniJSON.Json.Deserialize(message);
        }

        private static string SerializeDictionary(IDictionary<string, object> dict)
        {
            return MiniJSON.Json.Serialize(dict);
        }

        private static bool TryGetCallbackId(IDictionary<string, object> result, out string callbackId)
        {
            object callback;
            callbackId = null;
            if (result.TryGetValue("callback_id", out callback))
            {
                callbackId = callback as string;
                return true;
            }

            return false;
        }

        private static bool TryGetError(IDictionary<string, object> result, out string errorMessage)
        {
            object error;
            errorMessage = null;
            if (result.TryGetValue("error", out error))
            {
                errorMessage = error as string;
                return true;
            }

            return false;
        }
    }
}
