using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using FMODUnity;

public class FMODTools
{
    private static readonly string defaultFmodPath = "C:\\Program Files\\FMOD SoundSystem\\";

#if UNITY_EDITOR_WIN
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
#endif

    [MenuItem("FMOD/Download Banks", priority = -30)]
    public static void DownloadBanks()
    {
        DownloadBankFromLink("https://exploitavoid.com/storage/Master.bank?password=sndgmtk2025", "Master.bank");
        DownloadBankFromLink("https://exploitavoid.com/storage/Master.strings.bank?password=sndgmtk2025", "Master.strings.bank");
    }

    private static void DownloadBankFromLink(string url, string fileName)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        var request = www.SendWebRequest();
        request.completed += HandleFile;
        return;

        void HandleFile(AsyncOperation obj)
        {
            try
            {
                if (request.webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Download failed: " + www.error);
                }
                else
                {
                    var folderPath = Path.Combine(Application.dataPath, "..", "FMOD", "Build", "Desktop");
                    Directory.CreateDirectory(folderPath);
                    var filePath = Path.Combine(folderPath, fileName);
                    File.WriteAllBytes(filePath, request.webRequest.downloadHandler.data);
                    Debug.Log("Download successful: " + Path.Combine(folderPath, fileName));
                }
            }
            finally
            {
                request.webRequest.Dispose();
            }
        }
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
