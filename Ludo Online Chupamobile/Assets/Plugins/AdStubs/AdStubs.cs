using System;

namespace UnityEngine.Advertisements
{
    public enum ShowResult { Finished = 0, Skipped = 1, Failed = 2 }

    public class ShowOptions
    {
        public Action<ShowResult> resultCallback { get; set; }
    }

    public static class Advertisement
    {
        public static bool IsReady(string placementId)
        {
            return false;
        }

        public static void Show(string placementId, ShowOptions options)
        {
            if (options != null && options.resultCallback != null)
            {
                options.resultCallback(ShowResult.Failed);
            }
        }
    }
}
