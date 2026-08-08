/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /*
     * A short lived async request that loads a FBResult from a url endpoint
     */
    internal class AsyncRequestString : MonoBehaviour
    {
        private Uri url;
        private HttpMethod method;
        private IDictionary<string, string> formData;
        private WWWForm query;
        private FacebookDelegate<IGraphResult> callback;

        internal static void Post(
            Uri url,
            Dictionary<string, string> formData = null,
            FacebookDelegate<IGraphResult> callback = null)
        {
            Request(url, HttpMethod.POST, formData, callback);
        }

        internal static void Get(
            Uri url,
            Dictionary<string, string> formData = null,
            FacebookDelegate<IGraphResult> callback = null)
        {
            Request(url, HttpMethod.GET, formData, callback);
        }

        internal static void Request(
            Uri url,
            HttpMethod method,
            WWWForm query = null,
            FacebookDelegate<IGraphResult> callback = null)
        {
            ComponentFactory.AddComponent<AsyncRequestString>()
                .SetUrl(url)
                .SetMethod(method)
                .SetQuery(query)
                .SetCallback(callback);
        }

        internal static void Request(
            Uri url,
            HttpMethod method,
            IDictionary<string, string> formData = null,
            FacebookDelegate<IGraphResult> callback = null)
        {
            ComponentFactory.AddComponent<AsyncRequestString>()
                .SetUrl(url)
                .SetMethod(method)
                .SetFormData(formData)
                .SetCallback(callback);
        }

        internal IEnumerator Start()
        {
            WWW www;
            if (this.method == HttpMethod.GET)
            {
                string urlParams = this.url.AbsoluteUri.Contains("?") ? "&" : "?";
                if (this.formData != null)
                {
                    foreach (KeyValuePair<string, string> pair in this.formData)
                    {
                        urlParams += string.Format("{0}={1}&", Uri.EscapeDataString(pair.Key), Uri.EscapeDataString(pair.Value));
                    }
                }

                Dictionary<string, string> headers = new Dictionary<string, string>();

                // Unity has a bug where setting the headers on a get request fails when running on the webplayer
#if !UNITY_WEBPLAYER
                headers["User-Agent"] = Constants.GraphApiUserAgent;
#endif

                www = new WWW(this.url + urlParams, null, headers);
            }
            else
            {
                // POST or DELETE
                if (this.query == null)
                {
                    this.query = new WWWForm();
                }

                if (this.method == HttpMethod.DELETE)
                {
                    this.query.AddField("method", "delete");
                }

                if (this.formData != null)
                {
                    foreach (KeyValuePair<string, string> pair in this.formData)
                    {
                        this.query.AddField(pair.Key, pair.Value);
                    }
                }

                this.query.headers["User-Agent"] = Constants.GraphApiUserAgent;
                www = new WWW(this.url.AbsoluteUri, this.query);
            }

            yield return www;

            if (this.callback != null)
            {
                this.callback(new GraphResult(www));
            }

            // after the callback is called, www should be able to be disposed
            www.Dispose();
            MonoBehaviour.Destroy(this);
        }

        internal AsyncRequestString SetUrl(Uri url)
        {
            this.url = url;
            return this;
        }

        internal AsyncRequestString SetMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        internal AsyncRequestString SetFormData(IDictionary<string, string> formData)
        {
            this.formData = formData;
            return this;
        }

        internal AsyncRequestString SetQuery(WWWForm query)
        {
            this.query = query;
            return this;
        }

        internal AsyncRequestString SetCallback(FacebookDelegate<IGraphResult> callback)
        {
            this.callback = callback;
            return this;
        }
    }
}
