using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class AnalyticsManifestEditor
{
    public const string AppMetricaUrl = "https://github.com/appmetrica/appmetrica-unity-plugin.git#v6.10.0";
    public const string GameAnalyticsUrl = "https://github.com/GameAnalytics/GA-SDK-UNITY.git#8.0.1";
    public const string FirebaseVersion = "13.14.0";
    public const string DependencyManagerVersion = "1.2.187";
    public const string FirebaseFolder = "Assets/Firebase";
    public const string AppMetricaFolder = "Assets/appmetrica-unity-plugin-main";
    public const string GameAnalyticsFolder = "Assets/GA-SDK-UNITY-master";

    private const string RegistryName = "OpenUPM";
    private const string RegistryUrl = "https://package.openupm.com";
    private const string DependencyManagerPackage = "com.google.external-dependency-manager";
    private const string AppMetricaPackage = "io.appmetrica.analytics";
    private const string GameAnalyticsPackage = "com.gameanalytics.sdk";

    private static readonly string[] FirebasePackages =
    {
        "com.google.firebase.app",
        "com.google.firebase.analytics",
    };

    public static string GetFirebaseFileName(string package)
    {
        return GetFirebaseFileName(package, FirebaseVersion);
    }

    public static string GetFirebaseFileName(string package, string version)
    {
        return $"{package}-{version}.tgz";
    }

    public static string[] GetFirebasePackages()
    {
        return FirebasePackages;
    }

    public static string[] GetMissingFirebaseFiles(Func<string, bool> fileExists, Func<string, bool> folderExists)
    {
        return GetMissingFirebaseFiles(fileExists, folderExists, new AnalyticsSdkVersions());
    }

    public static string[] GetMissingFirebaseFiles(Func<string, bool> fileExists, Func<string, bool> folderExists,
        AnalyticsSdkVersions versions)
    {
        if (folderExists(FirebaseFolder))
            return new string[0];

        return FirebasePackages.Select(x => GetFirebaseFileName(x, versions.firebase))
            .Where(x => !fileExists($"LocalPackages/{x}"))
            .ToArray();
    }

    public static string AddSdks(string manifestJson, Func<string, bool> folderExists)
    {
        return AddSdks(manifestJson, folderExists, new AnalyticsSdkVersions());
    }

    public static string AddSdks(string manifestJson, Func<string, bool> folderExists, AnalyticsSdkVersions versions)
    {
        JObject manifest = JObject.Parse(manifestJson);
        if (!folderExists(AppMetricaFolder))
            AddDependency(manifest, AppMetricaPackage, versions.appMetricaUrl);

        if (!folderExists(GameAnalyticsFolder))
            AddDependency(manifest, GameAnalyticsPackage, versions.gameAnalyticsUrl);

        if (!folderExists(FirebaseFolder))
        {
            foreach (string package in FirebasePackages)
                AddDependency(manifest, package, $"file:../LocalPackages/{GetFirebaseFileName(package, versions.firebase)}");
        }

        AddDependency(manifest, DependencyManagerPackage, versions.dependencyManager);
        AddRegistry(manifest);
        return manifest.ToString(Formatting.Indented);
    }

    public static bool NeedsAutoInstall(string manifestJson, Func<string, bool> folderExists)
    {
        return !HasSdks(manifestJson, folderExists);
    }

    public static List<AnalyticsSdkChange> GetChanges(string manifestJson, Func<string, bool> folderExists,
        AnalyticsSdkVersions versions)
    {
        JObject dependencies = (JObject) JObject.Parse(manifestJson)["dependencies"];
        return GetPackages(folderExists, versions)
            .Where(x => dependencies[x.Key] != null && (string) dependencies[x.Key] != x.Value)
            .Select(x => new AnalyticsSdkChange {package = x.Key, current = (string) dependencies[x.Key], target = x.Value})
            .ToList();
    }

    public static string ApplyVersions(string manifestJson, Func<string, bool> folderExists, AnalyticsSdkVersions versions)
    {
        JObject manifest = JObject.Parse(manifestJson);
        foreach (AnalyticsSdkChange change in GetChanges(manifestJson, folderExists, versions))
            manifest["dependencies"][change.package] = change.target;

        return manifest.ToString(Formatting.Indented);
    }

    private static Dictionary<string, string> GetPackages(Func<string, bool> folderExists, AnalyticsSdkVersions versions)
    {
        Dictionary<string, string> packages = new Dictionary<string, string>
        {
            {DependencyManagerPackage, versions.dependencyManager},
        };

        if (!folderExists(AppMetricaFolder))
            packages[AppMetricaPackage] = versions.appMetricaUrl;

        if (!folderExists(GameAnalyticsFolder))
            packages[GameAnalyticsPackage] = versions.gameAnalyticsUrl;

        if (!folderExists(FirebaseFolder))
        {
            foreach (string package in FirebasePackages)
                packages[package] = $"file:../LocalPackages/{GetFirebaseFileName(package, versions.firebase)}";
        }

        return packages;
    }

    public static bool HasSdks(string manifestJson, Func<string, bool> folderExists)
    {
        JToken dependencies = JObject.Parse(manifestJson)["dependencies"];
        return (folderExists(AppMetricaFolder) || dependencies?[AppMetricaPackage] != null)
               && (folderExists(GameAnalyticsFolder) || dependencies?[GameAnalyticsPackage] != null)
               && (folderExists(FirebaseFolder) || FirebasePackages.All(x => dependencies?[x] != null));
    }

    private static void AddDependency(JObject manifest, string package, string version)
    {
        if (manifest["dependencies"] is not JObject dependencies)
        {
            dependencies = new JObject();
            manifest["dependencies"] = dependencies;
        }

        if (dependencies[package] == null)
            dependencies[package] = version;
    }

    private static void AddRegistry(JObject manifest)
    {
        if (manifest["scopedRegistries"] is not JArray registries)
        {
            registries = new JArray();
            manifest["scopedRegistries"] = registries;
        }

        bool hasScope = registries.OfType<JObject>().SelectMany(x => x["scopes"])
            .Any(x => DependencyManagerPackage.StartsWith((string) x));
        if (hasScope)
            return;

        JObject registry = registries.OfType<JObject>().FirstOrDefault(x => (string) x["url"] == RegistryUrl);
        if (registry == null)
        {
            registry = new JObject {["name"] = RegistryName, ["url"] = RegistryUrl, ["scopes"] = new JArray()};
            registries.Add(registry);
        }

        ((JArray) registry["scopes"]).Add(DependencyManagerPackage);
    }
}
