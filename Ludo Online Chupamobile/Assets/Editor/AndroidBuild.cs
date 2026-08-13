using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;

public class AndroidBuild
{
    // Call via Unity CLI: -executeMethod AndroidBuild.PerformAndroidBuild
    public static void PerformAndroidBuild()
    {
        var args = System.Environment.GetCommandLineArgs();
        string outputPath = null;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-buildOutput" && i + 1 < args.Length)
                outputPath = args[i + 1];
        }

        if (string.IsNullOrEmpty(outputPath))
        {
            outputPath = System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                "LudoAndroidBuild"
            );
        }

        System.IO.Directory.CreateDirectory(outputPath);

        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("No enabled scenes in Build Settings.");
            EditorApplication.Exit(1);
            return;
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = System.IO.Path.Combine(outputPath, "LudoOnline.apk"),
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        Debug.LogFormat("Starting Android build: output={0}", options.locationPathName);

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Android build succeeded: " + options.locationPathName);
        }
        else
        {
            Debug.LogError("Android build failed: " + summary.result + " - " + summary.totalErrors);
            EditorApplication.Exit(1);
        }
    }
}
