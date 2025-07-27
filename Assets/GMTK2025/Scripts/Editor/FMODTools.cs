using UnityEditor;
using UnityEngine;
using System.IO;
using FMODUnity;

public class FMODTools
{
    private static readonly string defaultFmodPath = "C:\\Program Files\\FMOD SoundSystem\\";

    [MenuItem("FMOD/Build Banks", priority = -20)]
    public static void PerformBuild()
    {
        if (!TryGetExecutablePath(out string studioPath))
        {
            Debug.LogError("Automatic banks build failed: Could not find FMOD Studio installation.");
        }
        string projectPath = Path.GetFullPath(Settings.Instance.SourceProjectPath);
        if (!File.Exists(projectPath))
        {
            Debug.LogError("Automatic banks build failed: Could not find project file.");
        }
        string command = $"/C \"\"{studioPath}\" -build \"{projectPath}\"\"";

        Debug.Log($"Starting bank build: {command}");
        System.Diagnostics.Process process = System.Diagnostics.Process.Start("CMD.exe", command);
        process.WaitForExit();
        Debug.Log($"Build process finished with {(process.ExitCode == 0 ? "success" : process.ExitCode)}.");
    }

    private static bool TryGetExecutablePath(out string path)
    {
        if (Directory.Exists(defaultFmodPath))
        {
            path = Path.Join(Directory.GetDirectories(defaultFmodPath)[0], "fmodstudio");
            return true;
        }
        path = null;
        return true;
    }
}
