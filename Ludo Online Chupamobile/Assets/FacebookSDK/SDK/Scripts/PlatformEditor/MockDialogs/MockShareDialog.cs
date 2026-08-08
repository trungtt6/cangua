/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Editor.Dialogs
{
    using System.Collections.Generic;
    using System.Text;

    internal class MockShareDialog : EditorFacebookMockDialog
    {
        public string SubTitle { private get; set; }

        protected override string DialogTitle
        {
            get
            {
                return "Mock " + this.SubTitle + " Dialog";
            }
        }

        protected override void DoGui()
        {
            // Empty
        }

        protected override void SendSuccessResult()
        {
            var result = new Dictionary<string, object>();

            if (FB.IsLoggedIn)
            {
                result["postId"] = this.GenerateFakePostID();
            }
            else
            {
                result["did_complete"] = true;
            }

            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                result[Constants.CallbackIdKey] = this.CallbackID;
            }

            if (this.Callback != null)
            {
                this.Callback(new ResultContainer(result));
            }
        }

        protected override void SendCancelResult()
        {
            var result = new Dictionary<string, object>();
            result[Constants.CancelledKey] = "true";
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                result[Constants.CallbackIdKey] = this.CallbackID;
            }

            this.Callback(new ResultContainer(result));
        }

        private string GenerateFakePostID()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(AccessToken.CurrentAccessToken.UserId);
            sb.Append('_');
            for (int i = 0; i < 17; i++)
            {
                sb.Append(UnityEngine.Random.Range(0, 10));
            }

            return sb.ToString();
        }
    }
}
