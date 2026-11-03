using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class AnalyticsManager : MonoSingleton<AnalyticsManager>
{
    public static event Action OnFirebaseInitialized;

    public bool isDevelop = false;
    protected List<AnalyticData> analytics = new List<AnalyticData>();
    public List<AnalyticTypeConfig> analyticsTypeConfig = new List<AnalyticTypeConfig>();
    private List<AnalyticType> analyticTypes = new List<AnalyticType>();


    protected override void OnCreateSingleton()
    {
        if (isDevelop)
        {
            Init();
        }
        else
        {
#if (!DEVELOP && !TESTER && !UNITY_EDITOR)
        Init();
#endif
        }
    }

    protected void Init()
    {
        foreach (var analyticsConfig in GetConfigs())
        {
            analyticTypes.Add(analyticsConfig.type);
            switch (analyticsConfig.type)
            {
                case AnalyticType.FirebaseAnalytics:
                {
#if ONEUP_FIREBASE
                    try
                    {
                        FirebaseAnalytic FirebaseAnalytic = new FirebaseAnalytic();
                        FirebaseAnalytic.Init(analyticsConfig.config);
                        AnalyticData analyticData = new AnalyticData();
                        analyticData.type = analyticsConfig.type;
                        analyticData.analytic = FirebaseAnalytic;
                        analytics.Add(analyticData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
#else
                    Debug.LogWarning("AnalyticsManager - FirebaseAnalytics SDK is not installed.");
#endif

                    break;
                }
                case AnalyticType.GameAnalytics:
                {
#if ONEUP_GAMEANALYTICS
                    try
                    {
                        GameAnalytic GameAnalytic = new GameAnalytic();
                        GameAnalytic.Init(analyticsConfig.config);
                        AnalyticData analyticData = new AnalyticData();
                        analyticData.type = analyticsConfig.type;
                        analyticData.analytic = GameAnalytic;
                        analytics.Add(analyticData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
#else
                    Debug.LogWarning("AnalyticsManager - GameAnalytics SDK is not installed.");
#endif

                    break;
                }
                case AnalyticType.AppMetrica:
                {
#if ONEUP_APPMETRICA
                    try
                    {
                        AppMetricaAnalytic AppMetricaAnalytic = new AppMetricaAnalytic();
                        AppMetricaAnalytic.Init(analyticsConfig.config);
                        AnalyticData analyticData = new AnalyticData();
                        analyticData.type = analyticsConfig.type;
                        analyticData.analytic = AppMetricaAnalytic;
                        analytics.Add(analyticData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
#else
                    Debug.LogWarning("AnalyticsManager - AppMetrica SDK is not installed.");
#endif

                    break;
                }
            }
        }
    }

    private List<AnalyticTypeConfig> GetConfigs()
    {
        if (analyticsTypeConfig.Count > 0)
            return analyticsTypeConfig;

        List<AnalyticTypeConfig> configs = new List<AnalyticTypeConfig>();
        foreach (AnalyticType type in Enum.GetValues(typeof(AnalyticType)))
        {
            BaseAnalyticsConfig config = Resources.Load<BaseAnalyticsConfig>(AnalyticsConfigPaths.GetResourcePath(type));
            if (config != null)
                configs.Add(new AnalyticTypeConfig {type = type, config = config});
        }

        return configs;
    }

    public string GetFirebaseInstanceId()
    {
#if ONEUP_FIREBASE
        return (GetAnalytic(AnalyticType.FirebaseAnalytics) as FirebaseAnalytic)?.GetAppInstanceId();
#else
        return null;
#endif
    }

    public string GetAppMetricaId()
    {
#if ONEUP_APPMETRICA
        return (GetAnalytic(AnalyticType.AppMetrica) as AppMetricaAnalytic)?.GetAppMetricaDeviceIdHash();
#else
        return null;
#endif
    }

    public static void NotifyFirebaseInitialized()
    {
        OnFirebaseInitialized?.Invoke();
    }

    public BaseAnalytic GetAnalytic(AnalyticType type)
    {
        return analytics.Find(x => x.type == type)?.analytic;
    }

    public void SendAnalyticEvent(string[] _name, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes ?? analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name);
        }
    }

    public void SendAnalyticEvent(string[] _name, float _parameterName, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name, _parameterName);
        }
    }

    public void SendAnalyticEvent(string _name, JObject _parameterData, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name, _parameterData);
        }
    }

    public void SendAnalyticEvent(string _name, string _parameterName, string _parameterValue,
        List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name, _parameterName, _parameterValue);
        }
    }

    public void SendAnalyticEvent(string _name, string _parameterName, double _parameterValue,
        List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name, _parameterName, _parameterValue);
        }
    }

    public void SendAnalyticEvent(string _name, string _parameterName, long _parameterValue,
        List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name, _parameterName, _parameterValue);
        }
    }

    public void SendAnalyticEvent(string _name, string _parameterName, int _parameterValue,
        List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name, _parameterName, _parameterValue);
        }
    }

    public void SendAnalyticEvent(string _name, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.SendAnalyticEvent(_name);
        }
    }

    public void SendAnalyticBusinessEvent(string _marketType, string _itemType, string _itemId, int _price,
        string _currency, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic
                .SendAnalyticBusinessEvent(_marketType, _itemType, _itemId, _price, _currency);
        }
    }

    public void ResetAnalyticsData(List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;
        foreach (var type in types)
        {
            analytics.Find(x => x.type == type)?.analytic.ResetAnalyticsData();
        }
    }

    public void SendProgressionEvent(ProgressionStatus status, string[] _name, int score,
        List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;

        foreach (var type in types)
            analytics.Find(x => x.type == type)?.analytic.SendProgressionEvent(status, _name, score);
    }


    public void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        int score, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;

        foreach (var type in types)
            analytics.Find(x => x.type == type)?.analytic
                .SendProgressionEvent(status, progression01, progression02, score);
    }

    public void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        string progression03, int score, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;

        foreach (var type in types)
            analytics.Find(x => x.type == type)?.analytic
                .SendProgressionEvent(status, progression01, progression02, progression03, score);
    }

    public void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemId,
        string itemType, List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0 || amount == 0)
            return;

        foreach (var type in types)
            analytics.Find(x => x.type == type)?.analytic
                .SendResourceEvent(status, currency, amount, itemType, itemId);
    }

    public void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType,
        string itemId, IDictionary<string, object> customFields, bool mergeFields = false,
        List<AnalyticType> _analyticTypes = null)
    {
        List<AnalyticType> types = _analyticTypes != null && _analyticTypes.Count > 0 ? _analyticTypes : analyticTypes;
        if (types == null || types.Count == 0)
            return;

        foreach (var type in types)
            analytics.Find(x => x.type == type)?.analytic.SendResourceEvent(status, currency, amount, itemType,
                itemId, customFields, mergeFields);
    }
}