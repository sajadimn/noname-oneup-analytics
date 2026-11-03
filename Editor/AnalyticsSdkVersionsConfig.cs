using UnityEditor;
using UnityEngine;

public class AnalyticsSdkVersionsConfig : ScriptableObject
{
    public const string AssetPath = "Assets/Settings/Analytics/AnalyticsSdkVersionsConfig.asset";

    public AnalyticsSdkVersions versions = new AnalyticsSdkVersions();

    public static AnalyticsSdkVersions Load()
    {
        AnalyticsSdkVersionsConfig config = AssetDatabase.LoadAssetAtPath<AnalyticsSdkVersionsConfig>(AssetPath);
        return config != null ? config.versions : new AnalyticsSdkVersions();
    }
}
