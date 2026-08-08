/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity.Editor
{
    using System.Collections.Generic;
    using UnityEngine;

    internal abstract class EditorFacebookMockDialog : MonoBehaviour
    {
        private Rect modalRect;
        private GUIStyle modalStyle;

        public Utilities.Callback<ResultContainer> Callback { protected get; set; }

        public string CallbackID { protected get; set; }

        protected abstract string DialogTitle { get; }

        public void Start()
        {
            this.modalRect = new Rect(10, 10, Screen.width - 20, Screen.height - 20);
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1.0f));
            texture.Apply();
            this.modalStyle = new GUIStyle();
            this.modalStyle.normal.background = texture;
        }

        public void OnGUI()
        {
            GUI.ModalWindow(
                this.GetHashCode(),
                this.modalRect,
                this.OnGUIDialog,
                this.DialogTitle,
                this.modalStyle);
        }

        protected abstract void DoGui();

        protected abstract void SendSuccessResult();

        protected virtual void SendCancelResult()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary[Constants.CancelledKey] = true;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                dictionary[Constants.CallbackIdKey] = this.CallbackID;
            }

            this.Callback(new ResultContainer(dictionary.ToJson()));
        }

        protected virtual void SendErrorResult(string errorMessage)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary[Constants.ErrorKey] = errorMessage;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                dictionary[Constants.CallbackIdKey] = this.CallbackID;
            }

            this.Callback(new ResultContainer(dictionary.ToJson()));
        }

        private void OnGUIDialog(int windowId)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("Warning! Mock dialog responses will NOT match production dialogs");
            GUILayout.Label("Test your app on one of the supported platforms");
            this.DoGui();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var loginLabel = new GUIContent("Send Success");
            var buttonRect = GUILayoutUtility.GetRect(loginLabel, GUI.skin.button);
            if (GUI.Button(buttonRect, loginLabel))
            {
                this.SendSuccessResult();
                MonoBehaviour.Destroy(this);
            }

            var cancelLabel = new GUIContent("Send Cancel");
            var cancelButtonRect = GUILayoutUtility.GetRect(cancelLabel, GUI.skin.button);
            if (GUI.Button(cancelButtonRect, cancelLabel, GUI.skin.button))
            {
                this.SendCancelResult();
                MonoBehaviour.Destroy(this);
            }

            var errorLabel = new GUIContent("Send Error");
            var errorButtonRect = GUILayoutUtility.GetRect(cancelLabel, GUI.skin.button);
            if (GUI.Button(errorButtonRect, errorLabel, GUI.skin.button))
            {
                this.SendErrorResult("Error: Error button pressed");
                MonoBehaviour.Destroy(this);
            }

            GUILayout.EndHorizontal();
        }
    }
}
