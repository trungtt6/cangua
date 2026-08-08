// Unity 6 removed the legacy UnityEngine.WWW class. This shim re-implements it on top of
// UnityWebRequest so that existing code that uses WWW continues to compile and run.

using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace UnityEngine
{
    /// <summary>
    /// Drop-in compatibility replacement for the removed <c>UnityEngine.WWW</c> class.
    /// Internally uses <see cref="UnityWebRequest"/>; can be used with <c>yield return</c>
    /// inside coroutines just like the original.
    /// </summary>
    public sealed class WWW : CustomYieldInstruction, IDisposable
    {
        private UnityWebRequest _req;

        // ── CustomYieldInstruction ──────────────────────────────────────────────
        public override bool keepWaiting => !_req.isDone;

        // ── Public API matching original WWW ───────────────────────────────────
        public string url { get; private set; }

        public bool isDone => _req.isDone;

        public string error
        {
            get
            {
                if (_req.result == UnityWebRequest.Result.Success) return null;
                return string.IsNullOrEmpty(_req.error) ? "Unknown error" : _req.error;
            }
        }

        public string text => _req.downloadHandler?.text;

        public byte[] bytes => _req.downloadHandler?.data;

        /// <summary>
        /// Decodes the downloaded bytes as a PNG/JPEG texture.
        /// Returns <c>null</c> when no data is available or the request failed.
        /// </summary>
        public Texture2D texture
        {
            get
            {
                var data = _req.downloadHandler?.data;
                if (data == null || data.Length == 0) return null;
                var tex = new Texture2D(2, 2);
                tex.LoadImage(data);
                return tex;
            }
        }

        // ── Constructors ───────────────────────────────────────────────────────

        /// <summary>Simple HTTP GET.</summary>
        public WWW(string url)
        {
            this.url = url;
            _req = UnityWebRequest.Get(url);
            _req.downloadHandler = new DownloadHandlerBuffer();
            _req.SendWebRequest();
        }

        /// <summary>HTTP POST using a <see cref="WWWForm"/>.</summary>
        public WWW(string url, WWWForm form)
        {
            this.url = url;
            _req = UnityWebRequest.Post(url, form);
            _req.SendWebRequest();
        }

        /// <summary>
        /// HTTP GET with custom headers when <paramref name="postData"/> is <c>null</c>;
        /// HTTP POST with raw body + custom headers when <paramref name="postData"/> is non-null.
        /// </summary>
        public WWW(string url, byte[] postData, Dictionary<string, string> headers)
        {
            this.url = url;
            if (postData != null && postData.Length > 0)
            {
                _req = new UnityWebRequest(url, "POST");
                _req.uploadHandler = new UploadHandlerRaw(postData);
            }
            else
            {
                _req = UnityWebRequest.Get(url);
            }
            _req.downloadHandler = new DownloadHandlerBuffer();
            if (headers != null)
                foreach (var kv in headers)
                    _req.SetRequestHeader(kv.Key, kv.Value);
            _req.SendWebRequest();
        }

        // ── IDisposable ────────────────────────────────────────────────────────
        public void Dispose()
        {
            _req?.Dispose();
        }
    }
}
