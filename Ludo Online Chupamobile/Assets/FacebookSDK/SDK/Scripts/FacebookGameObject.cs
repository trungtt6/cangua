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
    using UnityEngine;

    /// <summary>
    /// Init delegate.
    /// </summary>
    public delegate void InitDelegate();

    /// <summary>
    /// Facebook delegate.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <typeparam name="T">The result type.</typeparam>
    public delegate void FacebookDelegate<T>(T result) where T : IResult;

    /// <summary>
    /// Hide unity delegate.
    /// </summary>
    /// <param name="isUnityShown">When called with its sole argument set to false,
    /// your game should pause and prepare to lose focus. If it's called with its
    /// argument set to true, your game should prepare to regain focus and resume
    /// play. Your game should check whether it is in fullscreen mode when it resumes,
    /// and offer the player a chance to go to fullscreen mode if appropriate.</param>
    public delegate void HideUnityDelegate(bool isUnityShown);

    internal abstract class FacebookGameObject : MonoBehaviour, IFacebookCallbackHandler
    {
        public IFacebookImplementation Facebook { get; set; }

        public void Awake()
        {
            MonoBehaviour.DontDestroyOnLoad(this);
            AccessToken.CurrentAccessToken = null;

            // run whatever else needs to be setup
            this.OnAwake();
        }

        public void OnInitComplete(string message)
        {
            this.Facebook.OnInitComplete(new ResultContainer(message));
        }

        public void OnLoginComplete(string message)
        {
            this.Facebook.OnLoginComplete(new ResultContainer(message));
        }

        public void OnLogoutComplete(string message)
        {
            this.Facebook.OnLogoutComplete(new ResultContainer(message));
        }

        public void OnGetAppLinkComplete(string message)
        {
            this.Facebook.OnGetAppLinkComplete(new ResultContainer(message));
        }

        public void OnGroupCreateComplete(string message)
        {
            this.Facebook.OnGroupCreateComplete(new ResultContainer(message));
        }

        public void OnGroupJoinComplete(string message)
        {
            this.Facebook.OnGroupJoinComplete(new ResultContainer(message));
        }

        public void OnAppRequestsComplete(string message)
        {
            this.Facebook.OnAppRequestsComplete(new ResultContainer(message));
        }

        public void OnShareLinkComplete(string message)
        {
            this.Facebook.OnShareLinkComplete(new ResultContainer(message));
        }

        // use this to call the rest of the Awake function
        protected virtual void OnAwake()
        {
        }
    }
}
