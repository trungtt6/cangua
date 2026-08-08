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
    internal abstract class MobileFacebookGameObject : FacebookGameObject, IMobileFacebookCallbackHandler
    {
        private IMobileFacebookImplementation MobileFacebook
        {
            get
            {
                return (IMobileFacebookImplementation)this.Facebook;
            }
        }

        public void OnAppInviteComplete(string message)
        {
            this.MobileFacebook.OnAppInviteComplete(new ResultContainer(message));
        }

        public void OnFetchDeferredAppLinkComplete(string message)
        {
            this.MobileFacebook.OnFetchDeferredAppLinkComplete(new ResultContainer(message));
        }

        public void OnRefreshCurrentAccessTokenComplete(string message)
        {
            this.MobileFacebook.OnRefreshCurrentAccessTokenComplete(new ResultContainer(message));
        }
    }
}
