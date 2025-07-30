using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildTools
{
    public static void PerformBuild()
    {
#if UNITY_EDITOR_WIN
        FMODTools.PerformBuild();
        BuildPlayerOptions buildPlayerOptions = new()
        {
            scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
            locationPathName = "Build/WebGL",
            target = BuildTarget.WebGL,
        };
        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log(string.Join(",", buildReport.steps.Select(x => x.name)));
        Debug.Log(buildReport.summary);
#endif
    }
}
