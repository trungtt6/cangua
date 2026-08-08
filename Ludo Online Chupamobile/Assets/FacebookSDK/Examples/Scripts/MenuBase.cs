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
    using System.Linq;
    using UnityEngine;

    internal abstract class MenuBase : ConsoleBase
    {
        private static ShareDialogMode shareDialogMode;

        protected abstract void GetGui();

        protected virtual bool ShowDialogModeSelector()
        {
            return false;
        }

        protected virtual bool ShowBackButton()
        {
            return true;
        }

        protected void HandleResult(IResult result)
        {
            if (result == null)
            {
                this.LastResponse = "Null Response\n";
                LogView.AddLog(this.LastResponse);
                return;
            }

            this.LastResponseTexture = null;

            // Some platforms return the empty string instead of null.
            if (!string.IsNullOrEmpty(result.Error))
            {
                this.Status = "Error - Check log for details";
                this.LastResponse = "Error Response:\n" + result.Error;
            }
            else if (result.Cancelled)
            {
                this.Status = "Cancelled - Check log for details";
                this.LastResponse = "Cancelled Response:\n" + result.RawResult;
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                this.Status = "Success - Check log for details";
                this.LastResponse = "Success Response:\n" + result.RawResult;
            }
            else
            {
                this.LastResponse = "Empty Response\n";
            }

            LogView.AddLog(result.ToString());
        }

        protected void OnGUI()
        {
            if (this.IsHorizontalLayout())
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
            }

            GUILayout.Label(this.GetType().Name, this.LabelStyle);

            this.AddStatus();

            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 scrollPosition = this.ScrollPosition;
                scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
                this.ScrollPosition = scrollPosition;
            }
            #endif
            this.ScrollPosition = GUILayout.BeginScrollView(
                this.ScrollPosition,
                GUILayout.MinWidth(ConsoleBase.MainWindowFullWidth));

            GUILayout.BeginHorizontal();
            if (this.ShowBackButton())
            {
                this.AddBackButton();
            }

            this.AddLogButton();
            if (this.ShowBackButton())
            {
                // Fix GUILayout margin issues
                GUILayout.Label(GUIContent.none, GUILayout.MinWidth(ConsoleBase.MarginFix));
            }

            GUILayout.EndHorizontal();
            if (this.ShowDialogModeSelector())
            {
                this.AddDialogModeButtons();
            }

            GUILayout.BeginVertical();

            // Add the ui from decendants
            this.GetGui();
            GUILayout.Space(10);

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void AddStatus()
        {
            GUILayout.Space(5);
            GUILayout.Box("Status: " + this.Status, this.TextStyle, GUILayout.MinWidth(ConsoleBase.MainWindowWidth));
        }

        private void AddBackButton()
        {
            GUI.enabled = ConsoleBase.MenuStack.Any();
            if (this.Button("Back"))
            {
                this.GoBack();
            }

            GUI.enabled = true;
        }

        private void AddLogButton()
        {
            if (this.Button("Log"))
            {
                this.SwitchMenu(typeof(LogView));
            }
        }

        private void AddDialogModeButtons()
        {
            GUILayout.BeginHorizontal();
            foreach (var value in Enum.GetValues(typeof(ShareDialogMode)))
            {
                this.AddDialogModeButton((ShareDialogMode)value);
            }

            GUILayout.EndHorizontal();
        }

        private void AddDialogModeButton(ShareDialogMode mode)
        {
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && (mode != shareDialogMode);
            if (this.Button(mode.ToString()))
            {
                shareDialogMode = mode;
                FB.Mobile.ShareDialogMode = mode;
            }

            GUI.enabled = enabled;
        }
    }
}
