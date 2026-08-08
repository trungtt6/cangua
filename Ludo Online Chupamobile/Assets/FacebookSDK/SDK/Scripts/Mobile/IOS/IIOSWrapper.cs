/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Mobile.IOS
{
    internal interface IIOSWrapper
    {
        void Init(
            string appId,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix);

        void LogInWithReadPermissions(
            int requestId,
            string scope);

        void LogInWithPublishPermissions(
            int requestId,
            string scope);

        void LogOut();

        void SetShareDialogMode(int mode);

        void ShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL);

        void FeedShare(
            int requestId,
            string toId,
            string link,
            string linkName,
            string linkCaption,
            string linkDescription,
            string picture,
            string mediaSource);

        void AppRequest(
            int requestId,
            string message,
            string actionType,
            string objectId,
            string[] to = null,
            int toLength = 0,
            string filters = "",
            string[] excludeIds = null,
            int excludeIdsLength = 0,
            bool hasMaxRecipients = false,
            int maxRecipients = 0,
            string data = "",
            string title = "");

        void AppInvite(
            int requestId,
            string appLinkUrl,
            string previewImageUrl);

        void CreateGameGroup(
            int requestId,
            string name,
            string description,
            string privacy);

        void JoinGameGroup(int requestId, string groupId);

        void FBSettingsActivateApp(string appId);

        void LogAppEvent(
            string logEvent,
            double valueToSum,
            int numParams,
            string[] paramKeys,
            string[] paramVals);

        void LogPurchaseAppEvent(
            double logPurchase,
            string currency,
            int numParams,
            string[] paramKeys,
            string[] paramVals);

        void FBAppEventsSetLimitEventUsage(bool limitEventUsage);

        void GetAppLink(int requestId);

        void RefreshCurrentAccessToken(int requestId);

        string FBSdkVersion();

        void FetchDeferredAppLink(int requestId);
    }
}
