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
    using UnityEngine;

    internal class GameGroups : MenuBase
    {
        private string gamerGroupName = "Test group";
        private string gamerGroupDesc = "Test group for testing.";
        private string gamerGroupPrivacy = "closed";
        private string gamerGroupCurrentGroup = string.Empty;

        protected override void GetGui()
        {
            if (this.Button("Game Group Create - Closed"))
            {
                FB.GameGroupCreate(
                    "Test game group",
                    "Test description",
                    "CLOSED",
                    this.HandleResult);
            }

            if (this.Button("Game Group Create - Open"))
            {
                FB.GameGroupCreate(
                    "Test game group",
                    "Test description",
                    "OPEN",
                    this.HandleResult);
            }

            this.LabelAndTextField("Group Name", ref this.gamerGroupName);
            this.LabelAndTextField("Group Description", ref this.gamerGroupDesc);
            this.LabelAndTextField("Group Privacy", ref this.gamerGroupPrivacy);

            if (this.Button("Call Create Group Dialog"))
            {
                this.CallCreateGroupDialog();
            }

            this.LabelAndTextField("Group To Join", ref this.gamerGroupCurrentGroup);
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && !string.IsNullOrEmpty(this.gamerGroupCurrentGroup);
            if (this.Button("Call Join Group Dialog"))
            {
                this.CallJoinGroupDialog();
            }

            GUI.enabled = enabled && FB.IsLoggedIn;
            if (this.Button("Get All App Managed Groups"))
            {
                this.CallFbGetAllOwnedGroups();
            }

            if (this.Button("Get Gamer Groups Logged in User Belongs to"))
            {
                this.CallFbGetUserGroups();
            }

            if (this.Button("Make Group Post As User"))
            {
                this.CallFbPostToGamerGroup();
            }

            GUI.enabled = enabled;
        }

        private void GroupCreateCB(IGroupCreateResult result)
        {
            this.HandleResult(result);
            if (result.GroupId != null)
            {
                this.gamerGroupCurrentGroup = result.GroupId;
            }
        }

        private void GetAllGroupsCB(IGraphResult result)
        {
            if (!string.IsNullOrEmpty(result.RawResult))
            {
                this.LastResponse = result.RawResult;
                var resultDictionary = result.ResultDictionary;
                if (resultDictionary.ContainsKey("data"))
                {
                    var dataArray = (List<object>)resultDictionary["data"];

                    if (dataArray.Count > 0)
                    {
                        var firstGroup = (Dictionary<string, object>)dataArray[0];
                        this.gamerGroupCurrentGroup = (string)firstGroup["id"];
                    }
                }
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                this.LastResponse = result.Error;
            }
        }

        private void CallFbGetAllOwnedGroups()
        {
            FB.API(FB.AppId + "/groups", HttpMethod.GET, this.GetAllGroupsCB);
        }

        private void CallFbGetUserGroups()
        {
            FB.API("/me/groups?parent=" + FB.AppId, HttpMethod.GET, this.HandleResult);
        }

        private void CallCreateGroupDialog()
        {
            FB.GameGroupCreate(
                this.gamerGroupName,
                this.gamerGroupDesc,
                this.gamerGroupPrivacy,
                this.GroupCreateCB);
        }

        private void CallJoinGroupDialog()
        {
            FB.GameGroupJoin(
                this.gamerGroupCurrentGroup,
                this.HandleResult);
        }

        private void CallFbPostToGamerGroup()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["message"] = "herp derp a post";

            FB.API(
                this.gamerGroupCurrentGroup + "/feed",
                HttpMethod.POST,
                this.HandleResult,
                dict);
        }
    }
}
