using UnityEditor;

public static class AnalyticsConfigMenu
{
    public static readonly string[] Paths =
    {
        "Assets/Settings/Analytics/Resources/FireBaseAnalyticsConfig.asset",
        "Assets/Settings/Analytics/Resources/GameAnalyticsConfig.asset",
        "Assets/Settings/Analytics/Resources/AppMetricaAnalyticsConfig.asset",
        AnalyticsSdkVersionsConfig.AssetPath,
    };

    [MenuItem("OneUp/Analytics/Configs/Firebase")]
    private static void OpenFirebase()
    {
        Open("Assets/Settings/Analytics/Resources/FireBaseAnalyticsConfig.asset");
    }

    [MenuItem("OneUp/Analytics/Configs/GameAnalytics")]
    private static void OpenGameAnalytics()
    {
        Open("Assets/Settings/Analytics/Resources/GameAnalyticsConfig.asset");
    }

    [MenuItem("OneUp/Analytics/Configs/AppMetrica")]
    private static void OpenAppMetrica()
    {
        Open("Assets/Settings/Analytics/Resources/AppMetricaAnalyticsConfig.asset");
    }

    [MenuItem("OneUp/Analytics/Configs/SDK Versions")]
    private static void OpenSDKVersions()
    {
        Open(AnalyticsSdkVersionsConfig.AssetPath);
    }

    private static void Open(string path)
    {
        AnalyticsConfigGenerator.Generate();
        UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(path);
        Selection.activeObject = asset;
        EditorGUIUtility.PingObject(asset);
    }
}
