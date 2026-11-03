using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class AnalyticsConfigGenerator
{
    private const string ParentFolder = "Assets/Settings/Analytics/Resources";

    static AnalyticsConfigGenerator()
    {
        EditorApplication.delayCall += Generate;
    }

    [MenuItem("OneUp/Analytics/Create Missing Configs")]
    public static void Generate()
    {
        CreateFolder("Assets", "Settings");
        CreateFolder("Assets/Settings", "Analytics");
        CreateFolder("Assets/Settings/Analytics", "Resources");

        Create<FireBaseAnalyticsConfig>(AnalyticType.FirebaseAnalytics);
        Create<GameAnalyticsConfig>(AnalyticType.GameAnalytics);
        Create<AppMetricaAnalyticsConfig>(AnalyticType.AppMetrica);
        CreateVersionsConfig();
    }

    private static void CreateVersionsConfig()
    {
        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(AnalyticsSdkVersionsConfig.AssetPath, AssetPathToGUIDOptions.OnlyExistingAssets)))
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AnalyticsSdkVersionsConfig>(), AnalyticsSdkVersionsConfig.AssetPath);
    }

    private static void CreateFolder(string parent, string name)
    {
        if (!AssetDatabase.IsValidFolder($"{parent}/{name}"))
            AssetDatabase.CreateFolder(parent, name);
    }

    private static void Create<T>(AnalyticType type) where T : BaseAnalyticsConfig
    {
        string path = $"{ParentFolder}/{AnalyticsConfigPaths.GetFileName(type)}.asset";
        if (!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path, AssetPathToGUIDOptions.OnlyExistingAssets)))
            return;

        T config = ScriptableObject.CreateInstance<T>();
        config.type = type;
        AssetDatabase.CreateAsset(config, path);
        Debug.Log($"AnalyticsConfigGenerator - Created {path}");
    }
}
