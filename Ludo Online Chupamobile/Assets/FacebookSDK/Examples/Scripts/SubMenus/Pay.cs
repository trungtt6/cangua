/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Example
{
    using UnityEngine;

    internal class Pay : MenuBase
    {
        private string payProduct = string.Empty;

        protected override void GetGui()
        {
            this.LabelAndTextField("Product: ", ref this.payProduct);
            if (this.Button("Call Pay"))
            {
                this.CallFBPay();
            }

            GUILayout.Space(10);
        }

        private void CallFBPay()
        {
            FB.Canvas.Pay(this.payProduct, callback: this.HandleResult);
        }
    }
}
