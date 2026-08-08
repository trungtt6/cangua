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
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    internal class LogView : ConsoleBase
    {
        private static string datePatt = @"M/d/yyyy hh:mm:ss tt";
        private static IList<string> events = new List<string>();

        public static void AddLog(string log)
        {
            events.Insert(0, string.Format("{0}\n{1}\n", DateTime.Now.ToString(datePatt), log));
        }

        protected void OnGUI()
        {
            GUILayout.BeginVertical();
            if (this.Button("Back"))
            {
                this.GoBack();
            }

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

            GUILayout.TextArea(
                string.Join("\n", events.ToArray()),
                this.TextStyle,
                GUILayout.ExpandHeight(true),
                GUILayout.MaxWidth(ConsoleBase.MainWindowWidth));

            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }
    }
}
