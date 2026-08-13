// Minimal C# stub definitions for the Facebook Audience Network Unity SDK.
// The real SDK ships only as native libraries (.aar / .framework); there is no
// C# DLL in this project, so these stubs provide the type definitions that game
// scripts reference via "using AudienceNetwork;".
//
// If you later integrate a version of the SDK that includes a C# assembly, add the
// scripting define symbol AUDIENCE_NETWORK_SDK_PRESENT to your Player Settings and
// the stubs below will be excluded to avoid duplicate type errors.
using System;
using System.Collections;
using UnityEngine;

namespace AudienceNetwork
{
    public static class SdkVersion
    {
        public static string Build { get { return "stub"; } }
    }

    public enum AdSize
    {
        BANNER_HEIGHT_50,
        BANNER_HEIGHT_90,
        RECTANGLE_HEIGHT_250,
        CUSTOM
    }

    public class AdView : IDisposable
    {
        public Action AdViewDidLoad;
        public Action<string> AdViewDidFailWithError;
        public Action AdViewWillLogImpression;
        public Action AdViewDidClick;

        public AdView(string placementId, AdSize size) { }

        public void Register(GameObject go) { }

        public bool Show(int yOffset = 0) { return false; }

        public void Dispose() { }
    }

    public class InterstitialAd : IDisposable
    {
        public Action InterstitialAdDidLoad;
        public Action<string> InterstitialAdDidFailWithError;
        public Action InterstitialAdWillLogImpression;
        public Action InterstitialAdDidClick;
        // Note: lowercase 'i' intentional – matches the original SDK field name.
        public Action interstitialAdDidClose;

        public InterstitialAd(string placementId) { }

        public void Register(GameObject go) { }

        public void LoadAd() { }

        public bool Show() { return false; }

        public void Dispose() { }
    }

    public class NativeAd : IDisposable
    {
        public Action NativeAdDidLoad;
        public Action<string> NativeAdDidFailWithError;
        public Action NativeAdWillLogImpression;
        public Action NativeAdDidClick;

        public Sprite CoverImage { get; private set; }
        public Sprite IconImage { get; private set; }
        public string CoverImageURL { get; private set; }
        public string IconImageURL { get; private set; }

        public NativeAd(string placementId) { }

        public void Register(GameObject go) { }

        public IEnumerator LoadIconImage(string url) { yield break; }

        public IEnumerator LoadCoverImage(string url) { yield break; }

        public void Dispose() { }
    }
}
