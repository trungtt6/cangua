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

    internal class CanvasFacebookGameObject : FacebookGameObject, ICanvasFacebookCallbackHandler
    {
        protected ICanvasFacebookImplementation CanvasFacebookImpl
        {
            get
            {
                return (ICanvasFacebookImplementation)this.Facebook;
            }
        }

        public void OnPayComplete(string result)
        {
            this.CanvasFacebookImpl.OnPayComplete(new ResultContainer(result));
        }

        public void OnFacebookAuthResponseChange(string message)
        {
            this.CanvasFacebookImpl.OnFacebookAuthResponseChange(new ResultContainer(message));
        }

        public void OnUrlResponse(string message)
        {
            this.CanvasFacebookImpl.OnUrlResponse(message);
        }

        public void OnHideUnity(bool hide)
        {
            this.CanvasFacebookImpl.OnHideUnity(hide);
        }

        protected override void OnAwake()
        {
            // Facebook JS Bridge lives in it's own gameobject for optimization reasons
            // see UnityObject.SendMessage()
            var bridgeObject = new GameObject("FacebookJsBridge");
            bridgeObject.AddComponent<JsBridge>();
            bridgeObject.transform.parent = gameObject.transform;
        }
    }
}
