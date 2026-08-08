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
    using System.Collections;
    using UnityEngine;

    internal class GraphRequest : MenuBase
    {
        private string apiQuery = string.Empty;
        private Texture2D profilePic;

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && FB.IsLoggedIn;
            if (this.Button("Basic Request - Me"))
            {
                FB.API("/me", HttpMethod.GET, this.HandleResult);
            }

            if (this.Button("Retrieve Profile Photo"))
            {
                FB.API("/me/picture", HttpMethod.GET, this.ProfilePhotoCallback);
            }

            if (this.Button("Take and Upload screenshot"))
            {
                this.StartCoroutine(this.TakeScreenshot());
            }

            this.LabelAndTextField("Request", ref this.apiQuery);
            if (this.Button("Custom Request"))
            {
                FB.API(this.apiQuery, HttpMethod.GET, this.HandleResult);
            }

            if (this.profilePic != null)
            {
                GUILayout.Box(this.profilePic);
            }

            GUI.enabled = enabled;
        }

        private void ProfilePhotoCallback(IGraphResult result)
        {
            if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
            {
                this.profilePic = result.Texture;
            }

            this.HandleResult(result);
        }

        private IEnumerator TakeScreenshot()
        {
            yield return new WaitForEndOfFrame();

            var width = Screen.width;
            var height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            byte[] screenshot = tex.EncodeToPNG();

            var wwwForm = new WWWForm();
            wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
            wwwForm.AddField("message", "herp derp.  I did a thing!  Did I do this right?");
            FB.API("me/photos", HttpMethod.POST, this.HandleResult, wwwForm);
        }
    }
}
