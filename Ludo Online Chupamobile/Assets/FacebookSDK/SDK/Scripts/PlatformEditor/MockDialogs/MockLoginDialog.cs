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
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class MockLoginDialog : EditorFacebookMockDialog
    {
        private string accessToken = string.Empty;

        protected override string DialogTitle
        {
            get
            {
                return "Mock Login Dialog";
            }
        }

        protected override void DoGui()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("User Access Token:");
            this.accessToken = GUILayout.TextField(this.accessToken, GUI.skin.textArea, GUILayout.MinWidth(400));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Find Access Token"))
            {
                Application.OpenURL(string.Format("https://developers.facebook.com/tools/accesstoken/?app_id={0}", FB.AppId));
            }

            GUILayout.Space(20);
        }

        protected override void SendSuccessResult()
        {
            if (string.IsNullOrEmpty(this.accessToken))
            {
                this.SendErrorResult("Empty Access token string");
                return;
            }

            // Make a Graph API call to get FBID
            FB.API(
                "/me?fields=id&access_token=" + this.accessToken,
               HttpMethod.GET,
               delegate(IGraphResult graphResult)
            {
                if (!string.IsNullOrEmpty(graphResult.Error))
                {
                    this.SendErrorResult("Graph API error: " + graphResult.Error);
                    return;
                }

                string facebookID = graphResult.ResultDictionary["id"] as string;

                // Make a Graph API call to get Permissions
                FB.API(
                    "/me/permissions?access_token=" + this.accessToken,
                   HttpMethod.GET,
                   delegate(IGraphResult permResult)
                {
                    if (!string.IsNullOrEmpty(permResult.Error))
                    {
                        this.SendErrorResult("Graph API error: " + permResult.Error);
                        return;
                    }

                    // Parse permissions
                    List<string> grantedPerms = new List<string>();
                    List<string> declinedPerms = new List<string>();
                    var data = permResult.ResultDictionary["data"] as List<object>;
                    foreach (Dictionary<string, object> dict in data)
                    {
                        if (dict["status"] as string == "granted")
                        {
                            grantedPerms.Add(dict["permission"] as string);
                        }
                        else
                        {
                            declinedPerms.Add(dict["permission"] as string);
                        }
                    }

                    // Create Access Token
                    var newToken = new AccessToken(
                        this.accessToken,
                        facebookID,
                        DateTime.UtcNow.AddDays(60),
                        grantedPerms,
                        DateTime.UtcNow);

                    var result = (IDictionary<string, object>)MiniJSON.Json.Deserialize(newToken.ToJson());
                    result.Add("granted_permissions", grantedPerms);
                    result.Add("declined_permissions", declinedPerms);
                    if (!string.IsNullOrEmpty(this.CallbackID))
                    {
                        result[Constants.CallbackIdKey] = this.CallbackID;
                    }

                    if (this.Callback != null)
                    {
                        this.Callback(new ResultContainer(result));
                    }
                });
            });
        }
    }
}
