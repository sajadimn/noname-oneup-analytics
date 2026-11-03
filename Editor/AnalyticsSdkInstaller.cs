using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

[InitializeOnLoad]
public static class AnalyticsSdkInstaller
{
    private const string ManifestPath = "Packages/manifest.json";
    private const string LocalPackagesFolder = "LocalPackages";
    private const string FirebaseUrl = "https://dl.google.com/games/registry/unity/";
    private const string UpdateKey = "OneUp.Analytics.Update";
    private const string AttemptsKey = "OneUp.Analytics.InstallAttempts";
    private const string PartExtension = ".part";
    private const int MaxAttempts = 12;

    private static bool isRunning;
    private static UnityWebRequest request;
    private static string downloadPath;

    static AnalyticsSdkInstaller()
    {
        if (!Application.isBatchMode)
            EditorApplication.delayCall += RunNextStep;
    }

    [MenuItem("OneUp/Analytics/Install SDKs")]
    public static void Install()
    {
        SessionState.SetInt(AttemptsKey, 0);
        RunNextStep();
    }

    private static void RunNextStep()
    {
        int attempts = SessionState.GetInt(AttemptsKey, 0);
        if (isRunning || attempts >= MaxAttempts)
            return;

        string manifest = File.ReadAllText(ManifestPath);
        bool updating = SessionState.GetBool(UpdateKey, false);
        if (!updating && !AnalyticsManifestEditor.NeedsAutoInstall(manifest, Directory.Exists))
            return;

        string[] missingFiles = AnalyticsManifestEditor.GetMissingFirebaseFiles(File.Exists, Directory.Exists, AnalyticsSdkVersionsConfig.Load());

        isRunning = true;
        SessionState.SetInt(AttemptsKey, attempts + 1);
        Debug.Log("AnalyticsSdkInstaller - Installing missing analytics SDKs.");

        if (missingFiles.Length > 0)
            Download(missingFiles[0]);
        else if (updating)
            WriteUpdate();
        else
            WriteManifest();
    }

    [MenuItem("OneUp/Analytics/Update SDKs")]
    public static void UpdateSdks()
    {
        List<string> lines = AnalyticsManifestEditor
            .GetChanges(File.ReadAllText(ManifestPath), Directory.Exists, AnalyticsSdkVersionsConfig.Load())
            .Select(x => $"{x.package}: {x.current} > {x.target}").ToList();

        if (lines.Count == 0)
        {
            EditorUtility.DisplayDialog("Update SDKs", "All SDKs already match AnalyticsSdkVersionsConfig.", "OK");
            return;
        }

        if (EditorUtility.DisplayDialog("Update SDKs", string.Join("\n", lines), "Update", "Cancel"))
            ApplyUpdate();
    }

    public static void ApplyUpdate()
    {
        SessionState.SetBool(UpdateKey, true);
        SessionState.SetInt(AttemptsKey, 0);
        RunNextStep();
    }

    private static void WriteUpdate()
    {
        AnalyticsSdkVersions versions = AnalyticsSdkVersionsConfig.Load();
        string manifest = File.ReadAllText(ManifestPath);
        File.WriteAllText(ManifestPath, AnalyticsManifestEditor.ApplyVersions(manifest, Directory.Exists, versions));
        RemoveOldFirebaseFiles(versions);
        SessionState.EraseBool(UpdateKey);
        isRunning = false;
        Client.Resolve();
    }

    private static void RemoveOldFirebaseFiles(AnalyticsSdkVersions versions)
    {
        foreach (string package in AnalyticsManifestEditor.GetFirebasePackages())
        {
            string current = AnalyticsManifestEditor.GetFirebaseFileName(package, versions.firebase);
            foreach (string file in Directory.GetFiles(LocalPackagesFolder, $"{package}-*.tgz"))
            {
                if (Path.GetFileName(file) != current)
                    File.Delete(file);
            }
        }
    }

    private static void WriteManifest()
    {
        string manifest = File.ReadAllText(ManifestPath);
        File.WriteAllText(ManifestPath, AnalyticsManifestEditor.AddSdks(manifest, Directory.Exists, AnalyticsSdkVersionsConfig.Load()));
        isRunning = false;
        Client.Resolve();
    }

    private static void Download(string fileName)
    {
        string package = fileName.Replace($"-{AnalyticsSdkVersionsConfig.Load().firebase}.tgz", "");
        Directory.CreateDirectory(LocalPackagesFolder);
        downloadPath = Path.Combine(LocalPackagesFolder, fileName);
        request = UnityWebRequest.Get($"{FirebaseUrl}{package}/{fileName}");
        request.downloadHandler = new DownloadHandlerFile(downloadPath + PartExtension);
        request.SendWebRequest();
        EditorApplication.update += WaitForDownload;
    }

    private static void WaitForDownload()
    {
        if (!request.isDone)
            return;

        EditorApplication.update -= WaitForDownload;
        bool success = request.result == UnityWebRequest.Result.Success;
        string error = request.error;
        request.Dispose();
        request = null;
        isRunning = false;

        if (success)
        {
            File.Move(downloadPath + PartExtension, downloadPath);
            EditorApplication.delayCall += RunNextStep;
            return;
        }

        File.Delete(downloadPath + PartExtension);
        SessionState.SetInt(AttemptsKey, MaxAttempts);
        Debug.LogError($"AnalyticsSdkInstaller - Failed to download {downloadPath}: {error} Run OneUp/Analytics/Install SDKs to try again.");
    }
}
