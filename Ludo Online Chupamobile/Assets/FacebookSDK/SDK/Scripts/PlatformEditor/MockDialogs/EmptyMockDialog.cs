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

    internal class EmptyMockDialog : EditorFacebookMockDialog
    {
        public string EmptyDialogTitle { get; set; }

        protected override string DialogTitle
        {
            get
            {
                return this.EmptyDialogTitle;
            }
        }

        protected override void DoGui()
        {
            // Empty
        }

        protected override void SendSuccessResult()
        {
            var result = new Dictionary<string, object>();
            result["did_complete"] = true;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                result[Constants.CallbackIdKey] = this.CallbackID;
            }

            if (this.Callback != null)
            {
                this.Callback(new ResultContainer(result));
            }
        }
    }
}
