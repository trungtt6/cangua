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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class DialogShare : MenuBase
    {
        // Custom Share Link
        private string shareLink = "https://developers.facebook.com/";
        private string shareTitle = "Link Title";
        private string shareDescription = "Link Description";
        private string shareImage = "http://i.imgur.com/j4M7vCO.jpg";

        // Custom Feed Share
        private string feedTo = string.Empty;
        private string feedLink = "https://developers.facebook.com/";
        private string feedTitle = "Test Title";
        private string feedCaption = "Test Caption";
        private string feedDescription = "Test Description";
        private string feedImage = "http://i.imgur.com/zkYlB.jpg";
        private string feedMediaSource = string.Empty;

        protected override bool ShowDialogModeSelector()
        {
            #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            return true;
            #else
            return false;
            #endif
        }

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            if (this.Button("Share - Link"))
            {
                FB.ShareLink(new Uri("https://developers.facebook.com/"), callback: this.HandleResult);
            }

            // Note: Web dialog doesn't support photo urls.
            if (this.Button("Share - Link Photo"))
            {
                FB.ShareLink(
                    new Uri("https://developers.facebook.com/"),
                    "Link Share",
                    "Look I'm sharing a link",
                    new Uri("http://i.imgur.com/j4M7vCO.jpg"),
                    callback: this.HandleResult);
            }

            this.LabelAndTextField("Link", ref this.shareLink);
            this.LabelAndTextField("Title", ref this.shareTitle);
            this.LabelAndTextField("Description", ref this.shareDescription);
            this.LabelAndTextField("Image", ref this.shareImage);
            if (this.Button("Share - Custom"))
            {
                FB.ShareLink(
                    new Uri(this.shareLink),
                    this.shareTitle,
                    this.shareDescription,
                    new Uri(this.shareImage),
                    this.HandleResult);
            }

            GUI.enabled = enabled && (!Constants.IsEditor || (Constants.IsEditor && FB.IsLoggedIn));
            if (this.Button("Feed Share - No To"))
            {
                FB.FeedShare(
                    string.Empty,
                    new Uri("https://developers.facebook.com/"),
                    "Test Title",
                    "Test caption",
                    "Test Description",
                    new Uri("http://i.imgur.com/zkYlB.jpg"),
                    string.Empty,
                    this.HandleResult);
            }

            this.LabelAndTextField("To", ref this.feedTo);
            this.LabelAndTextField("Link", ref this.feedLink);
            this.LabelAndTextField("Title", ref this.feedTitle);
            this.LabelAndTextField("Caption", ref this.feedCaption);
            this.LabelAndTextField("Description", ref this.feedDescription);
            this.LabelAndTextField("Image", ref this.feedImage);
            this.LabelAndTextField("Media Source", ref this.feedMediaSource);
            if (this.Button("Feed Share - Custom"))
            {
                FB.FeedShare(
                    this.feedTo,
                    string.IsNullOrEmpty(this.feedLink) ? null : new Uri(this.feedLink),
                    this.feedTitle,
                    this.feedCaption,
                    this.feedDescription,
                    string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage),
                    this.feedMediaSource,
                    this.HandleResult);
            }

            GUI.enabled = enabled;
        }
    }
}
