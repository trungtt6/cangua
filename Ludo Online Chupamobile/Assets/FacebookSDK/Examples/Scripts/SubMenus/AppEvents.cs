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
    using System.Collections.Generic;

    internal class AppEvents : MenuBase
    {
        protected override void GetGui()
        {
            if (this.Button("Log FB App Event"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.LogAppEvent(
                    AppEventName.UnlockedAchievement,
                    null,
                    new Dictionary<string, object>()
                    {
                        { AppEventParameterName.Description, "Clicked 'Log AppEvent' button" }
                    });
                LogView.AddLog(
                    "You may see results showing up at https://www.facebook.com/analytics/"
                    + FB.AppId);
            }
        }
    }
}
