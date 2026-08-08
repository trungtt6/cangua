/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

/// <summary>
/// Audience Network console endpoint. This endpoint exposes some internal functionality
/// that can be called with unity from the command line. This class is public and static
/// outside of a namespace to enable it to be called by unity.
/// </summary>
using AudienceNetwork.Editor;

public static class AudienceNetworkConsoleEndpoint
{
    public static void ExportPackage()
    {
        AudienceNetworkBuild.ExportPackage();
    }
}
