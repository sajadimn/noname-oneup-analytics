public static class AnalyticsConfigPaths
{
    public static string GetFileName(AnalyticType type)
    {
        switch (type)
        {
            case AnalyticType.FirebaseAnalytics:
                return "FireBaseAnalyticsConfig";
            case AnalyticType.GameAnalytics:
                return "GameAnalyticsConfig";
            default:
                return "AppMetricaAnalyticsConfig";
        }
    }

    public static string GetResourcePath(AnalyticType type)
    {
        return GetFileName(type);
    }
}
