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
public enum AdAction
{
    Undefined = 0,
    Clicked = 1,
    Show = 2,
    FailedShow = 3,
    RewardReceived = 4,
    Request = 5,
    Loaded = 6
}

[Serializable]
public enum AdType
{
    Undefined = 0,
    Video = 1,
    RewardedVideo = 2,
    Playable = 3,
    Interstitial = 4,
    OfferWall = 5,
    Banner = 6,
    AppOpen = 7
}

[Serializable]
public enum AdError
{
    Undefined = 0,
    Unknown = 1,
    Offline = 2,
    NoFill = 3,
    InternalError = 4,
    InvalidRequest = 5,
    UnableToPrecache = 6
}

[Serializable]
public class AnalyticTypeConfig
{
    public AnalyticType type;
    public BaseAnalyticsConfig config;
}
