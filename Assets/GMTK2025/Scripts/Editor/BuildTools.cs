using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.Build.Reporting;

public class BuildTools
{
    public static void PerformBuild()
    {
        FMODTools.PerformBuild();
        BuildPlayerOptions buildPlayerOptions = new()
        {
            scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
            locationPathName = "Build/WebGL",
            target = BuildTarget.WebGL
        };
        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log(string.Join(",", buildReport.steps.Select(x => x.name)));
        Debug.Log(buildReport.summary);
    }
}
