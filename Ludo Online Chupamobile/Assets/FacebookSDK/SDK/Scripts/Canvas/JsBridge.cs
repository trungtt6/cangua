/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Canvas
{
    using UnityEngine;

    internal class JsBridge : MonoBehaviour
    {
        private ICanvasFacebookCallbackHandler facebook;

        public void Start()
        {
            this.facebook = ComponentFactory.GetComponent<CanvasFacebookGameObject>(
                ComponentFactory.IfNotExist.ReturnNull);
        }

        public void OnLoginComplete(string responseJsonData = "")
        {
            this.facebook.OnLoginComplete(responseJsonData);
        }

        public void OnFacebookAuthResponseChange(string responseJsonData = "")
        {
            this.facebook.OnFacebookAuthResponseChange(responseJsonData);
        }

        public void OnPayComplete(string responseJsonData = "")
        {
            this.facebook.OnPayComplete(responseJsonData);
        }

        public void OnAppRequestsComplete(string responseJsonData = "")
        {
            this.facebook.OnAppRequestsComplete(responseJsonData);
        }

        public void OnShareLinkComplete(string responseJsonData = "")
        {
            this.facebook.OnShareLinkComplete(responseJsonData);
        }

        public void OnGroupCreateComplete(string responseJsonData = "")
        {
            this.facebook.OnGroupCreateComplete(responseJsonData);
        }

        public void OnJoinGroupComplete(string responseJsonData = "")
        {
            this.facebook.OnGroupJoinComplete(responseJsonData);
        }

        public void OnFacebookFocus(string state)
        {
            this.facebook.OnHideUnity(state != "hide");
        }

        public void OnInitComplete(string responseJsonData = "")
        {
            this.facebook.OnInitComplete(responseJsonData);
        }

        public void OnUrlResponse(string url = "")
        {
            this.facebook.OnUrlResponse(url);
        }
    }
}
