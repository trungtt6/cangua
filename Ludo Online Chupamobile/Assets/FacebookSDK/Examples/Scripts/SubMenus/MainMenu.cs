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
    using System.Linq;
    using UnityEngine;

    internal sealed class MainMenu : MenuBase
    {
        protected override bool ShowBackButton()
        {
            return false;
        }

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            if (this.Button("FB.Init"))
            {
                FB.Init(this.OnInitComplete, this.OnHideUnity);
                this.Status = "FB.Init() called with " + FB.AppId;
            }

            GUILayout.BeginHorizontal();

            GUI.enabled = enabled && FB.IsInitialized;
            if (this.Button("Login"))
            {
                this.CallFBLogin();
                this.Status = "Login called";
            }

            GUI.enabled = FB.IsLoggedIn;
            if (this.Button("Get publish_actions"))
            {
                this.CallFBLoginForPublish();
                this.Status = "Login (for publish_actions) called";
            }

            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR
            if (this.Button("Logout"))
            {
                CallFBLogout();
                this.Status = "Logout called";
            }
            #endif
            // Fix GUILayout margin issues
            GUILayout.Label(GUIContent.none, GUILayout.MinWidth(ConsoleBase.MarginFix));
            GUILayout.EndHorizontal();

            GUI.enabled = enabled && FB.IsInitialized;
            if (this.Button("Share Dialog"))
            {
                this.SwitchMenu(typeof(DialogShare));
            }

            bool savedEnabled = GUI.enabled;
            GUI.enabled = enabled &&
                AccessToken.CurrentAccessToken != null &&
                AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions");
            if (this.Button("Game Groups"))
            {
                this.SwitchMenu(typeof(GameGroups));
            }

            GUI.enabled = savedEnabled;

            if (this.Button("App Requests"))
            {
                this.SwitchMenu(typeof(AppRequests));
            }

            if (this.Button("Graph Request"))
            {
                this.SwitchMenu(typeof(GraphRequest));
            }

            if (Constants.IsWeb && this.Button("Pay"))
            {
                this.SwitchMenu(typeof(Pay));
            }

            if (this.Button("App Events"))
            {
                this.SwitchMenu(typeof(AppEvents));
            }

            if (this.Button("App Links"))
            {
                this.SwitchMenu(typeof(AppLinks));
            }

            if (Constants.IsMobile && this.Button("App Invites"))
            {
                this.SwitchMenu(typeof(AppInvites));
            }

            if (Constants.IsMobile && this.Button("Access Token"))
            {
                this.SwitchMenu(typeof(AccessTokenMenu));
            }

            GUI.enabled = enabled;
        }

        private void CallFBLogin()
        {
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
        }

        private void CallFBLoginForPublish()
        {
            // It is generally good behavior to split asking for read and publish
            // permissions rather than ask for them all at once.
            //
            // In your own game, consider postponing this call until the moment
            // you actually need it.
            FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, this.HandleResult);
        }

        private void CallFBLogout()
        {
            FB.LogOut();
        }

        private void OnInitComplete()
        {
            this.Status = "Success - Check log for details";
            this.LastResponse = "Success Response: OnInitComplete Called\n";
            string logMessage = string.Format(
                "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
                FB.IsLoggedIn,
                FB.IsInitialized);
            LogView.AddLog(logMessage);
            if (AccessToken.CurrentAccessToken != null)
            {
                LogView.AddLog(AccessToken.CurrentAccessToken.ToString());
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            this.Status = "Success - Check log for details";
            this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
            LogView.AddLog("Is game shown: " + isGameShown);
        }
    }
}
