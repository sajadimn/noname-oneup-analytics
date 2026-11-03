using System;

[Serializable]
public class AnalyticsSdkVersions
{
    public string appMetricaUrl = AnalyticsManifestEditor.AppMetricaUrl;
    public string gameAnalyticsUrl = AnalyticsManifestEditor.GameAnalyticsUrl;
    public string firebase = AnalyticsManifestEditor.FirebaseVersion;
    public string dependencyManager = AnalyticsManifestEditor.DependencyManagerVersion;
}
