using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;

public class IntegrationBuild
{
    // Call via Unity CLI: -executeMethod IntegrationBuild.PerformIntegrationBuild
    // Supports custom args: -buildOutput <path> -buildTarget <Win|Linux|Mac>
    public static void PerformIntegrationBuild()
    {
        var args = System.Environment.GetCommandLineArgs();
        string outputPath = null;
        string buildTarget = "Win";

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-buildOutput" && i + 1 < args.Length)
                outputPath = args[i + 1];
            if (args[i] == "-buildTarget" && i + 1 < args.Length)
                buildTarget = args[i + 1];
        }

        if (string.IsNullOrEmpty(outputPath))
        {
            outputPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "LudoIntegrationBuild");
        }

        // Ensure folder exists
        System.IO.Directory.CreateDirectory(outputPath);

        // Get enabled scenes
        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        if (scenes.Length == 0)
        {
            Debug.LogError("No enabled scenes in Build Settings. Please enable MenuScene and relevant scenes.");
            return;
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;

        // Choose target and output executable name
        if (buildTarget.Equals("Linux", System.StringComparison.OrdinalIgnoreCase))
        {
            buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
            buildPlayerOptions.locationPathName = System.IO.Path.Combine(outputPath, "LudoIntegration.x86_64");
        }
        else if (buildTarget.Equals("Mac", System.StringComparison.OrdinalIgnoreCase) || buildTarget.Equals("OSX", System.StringComparison.OrdinalIgnoreCase))
        {
            buildPlayerOptions.target = BuildTarget.StandaloneOSX;
            buildPlayerOptions.locationPathName = System.IO.Path.Combine(outputPath, "LudoIntegration.app");
        }
        else
        {
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.locationPathName = System.IO.Path.Combine(outputPath, "LudoIntegration.exe");
        }

        buildPlayerOptions.options = BuildOptions.None;

        Debug.LogFormat("Starting build: target={0} output={1}", buildPlayerOptions.target, buildPlayerOptions.locationPathName);

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Integration build succeeded: " + buildPlayerOptions.locationPathName);
        }
        else
        {
            Debug.LogError("Integration build failed: " + summary.result + " - " + summary.totalErrors);
            // Fail the editor process with non-zero exit for CI
            EditorApplication.Exit(1);
        }
    }
}
