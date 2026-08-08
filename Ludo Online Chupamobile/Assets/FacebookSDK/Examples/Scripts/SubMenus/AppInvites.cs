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
    using System;

    internal class AppInvites : MenuBase
    {
        protected override void GetGui()
        {
            if (this.Button("Android Invite"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), callback: this.HandleResult);
            }

            if (this.Button("Android Invite With Custom Image"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), new Uri("http://i.imgur.com/zkYlB.jpg"), this.HandleResult);
            }

            if (this.Button("iOS Invite"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), callback: this.HandleResult);
            }

            if (this.Button("iOS Invite With Custom Image"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), new Uri("http://i.imgur.com/zkYlB.jpg"), this.HandleResult);
            }
        }
    }
}
