using System;

namespace UnityEngine.Advertisements
{
    // Minimal shims to allow code that references Unity Ads types to compile when the package
    // is not present in the editor/CI environment.
    public enum ShowResult
    {
        Finished,
        Skipped,
        Failed
    }

    public class ShowOptions
    {
        public Action<ShowResult> resultCallback;
    }

    public static class Advertisement
    {
        public static bool IsReady(string placementId)
        {
            return false;
        }

        public static void Show(string placementId, ShowOptions options)
        {
            // Immediately invoke failure callback so callers handle the no-fill case deterministically.
            if (options != null && options.resultCallback != null)
            {
                options.resultCallback(ShowResult.Failed);
            }
        }
    }
}
