using System;

public enum AnalyticType
{
    FirebaseAnalytics = 0,
    GameAnalytics = 1,
    AppMetrica = 2,
}

[Serializable]
public class AnalyticData
{
    public AnalyticType type;
    public BaseAnalytic analytic;
}

[Serializable]
public enum ResourceFlowType
{
    //Undefined progression
    Undefined = 0,
    // Source: Used when adding resource to a user
    Source = 1,
    // Sink: Used when removing a resource from a user
    Sink = 2
}

[Serializable]
public enum ProgressionStatus
{
    //Undefined progression
    Undefined = 0,
    // User started progression
    Start = 1,
    // User succesfully ended a progression
    Complete = 2,
    // User failed a progression
    Fail = 3
}
[Serializable]
public class AnalyticTypeConfig
{
    public AnalyticType type;
    public BaseAnalyticsConfig config;
}
