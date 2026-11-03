#if ONEUP_GAMEANALYTICS
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using UnityEngine;

public class GameAnalytic : BaseAnalytic
{
    protected bool gameAnalyticsInitialized = false;
    private GameAnalyticsConfig gameAnalyticsConfig;

    public override void Init(BaseAnalyticsConfig config)
    {
        GameObject gameAnalytic = new GameObject();
        gameAnalyticsConfig = config as GameAnalyticsConfig;
        gameAnalytic.name = "GameAnalytic Services";
        gameAnalytic.AddComponent<GameAnalytics>();
        Initialize();
    }

    protected override void Initialize()
    {
        DebugLog("Enabling GameAnalytics Data Collection");
        GA_Setup.SetAvailableResourceCurrencies(gameAnalyticsConfig.availableResourceCurrencies);
        GA_Setup.SetAvailableResourceItemTypes(gameAnalyticsConfig.availableResourceItemTypes);

        GameAnalytics.Initialize();
        gameAnalyticsInitialized = true;
    }


    public override void SendAnalyticEvent(string[] _name)
    {
        if (!gameAnalyticsInitialized)
            return;
        string eventName = "";
        foreach (var key in _name)
        {
            eventName += key + ":";
        }

        eventName = eventName.Remove(eventName.Length - 1);
        GameAnalytics.NewDesignEvent(eventName);
        DebugLog(string.Format("Send Design Event : {0}", eventName));
    }

    public override void SendAnalyticEvent(string[] _name, float _parameterValue)
    {
        if (!gameAnalyticsInitialized)
            return;
        string eventName = "";
        foreach (var key in _name)
        {
            eventName += key + ":";
        }

        eventName = eventName.Remove(eventName.Length - 1);
        GameAnalytics.NewDesignEvent(eventName, _parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1}", eventName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, JObject _parameters)
    {
        if (!gameAnalyticsInitialized)
            return;
        Dictionary<string, object> fields = _parameters.ToObject<Dictionary<string, object>>();

        GameAnalytics.NewDesignEvent(_name, fields);
        DebugLog(string.Format("Send Design Event : {0} : {1}", _name, _parameters));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, string _parameterValue)
    {
        if (!gameAnalyticsInitialized)
            return;
        string eventName = _name + ":" + _parameterName + ":" + _parameterValue;
        GameAnalytics.NewDesignEvent(eventName);
        DebugLog(string.Format("Send Design Event : {0}", eventName));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, double _parameterValue)
    {
        if (!gameAnalyticsInitialized)
            return;
        string eventName = _name + ":" + _parameterName;
        GameAnalytics.NewDesignEvent(eventName, (float)_parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1}", eventName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, long _parameterValue)
    {
        if (!gameAnalyticsInitialized)
            return;
        string eventName = _name + ":" + _parameterName;
        GameAnalytics.NewDesignEvent(eventName, (float)_parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1}", eventName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, int _parameterValue)
    {
        if (!gameAnalyticsInitialized)
            return;
        string eventName = _name + ":" + _parameterName;
        GameAnalytics.NewDesignEvent(eventName, (float)_parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1}", eventName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name)
    {
        if (!gameAnalyticsInitialized)
            return;
        GameAnalytics.NewDesignEvent(_name);
        DebugLog(string.Format("Send Design Event : {0}", _name));
    }

    public override void SendAnalyticBusinessEvent(string _marketType, string _itemType, string _itemId, int _price,
        string _currency)
    {
        if (!gameAnalyticsInitialized)
            return;

        GameAnalytics.NewBusinessEvent(_currency, _price, _itemType, _itemId, _marketType);
        DebugLog(string.Format("Send Business Event : {0} : {1} : {2} : {3} : {4}", _marketType,
            _itemType, _itemId,
            _price, _currency));
    }

    public override void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType,
        string itemId)
    {
        if (!gameAnalyticsInitialized)
            return;

        GAResourceFlowType resourceStatus = (GAResourceFlowType)((int)status);
        GameAnalytics.NewResourceEvent(resourceStatus, currency, amount, itemType, itemId);
        DebugLog(
            $"Send Resource Event : {resourceStatus} Currency: {currency} Amount: {amount} ItemType: {itemType} ItemId: {itemId}");
    }

    public override void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType,
        string itemId, IDictionary<string, object> customFields, bool mergeFields = false)
    {
        if (!gameAnalyticsInitialized)
            return;

        GAResourceFlowType resourceStatus = (GAResourceFlowType)((int)status);
        GameAnalytics.NewResourceEvent(resourceStatus, currency, amount, itemType, itemId, customFields,
            mergeFields);
        DebugLog(
            $"Send Resource Event : {resourceStatus} Currency: {currency} Amount: {amount} ItemType: {itemType} ItemId: {itemId}");
    }

    public override void SendProgressionEvent(ProgressionStatus status, string[] _name, int score)
    {
        if (!gameAnalyticsInitialized)
            return;

        GAProgressionStatus progressionStatus = (GAProgressionStatus)((int)status);
        string eventName = "";
        foreach (var key in _name)
            eventName += key + "_";

        eventName = eventName.Remove(eventName.Length - 1);
        GameAnalytics.NewProgressionEvent(progressionStatus, eventName, score);
        DebugLog($"Send Progression Event : {progressionStatus} EventName: {eventName} Score: {score}");
    }

    public override void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        int score)
    {
        if (!gameAnalyticsInitialized)
            return;

        GAProgressionStatus progressionStatus = (GAProgressionStatus)((int)status);
        GameAnalytics.NewProgressionEvent(progressionStatus, progression01, progression02, score);
        DebugLog(
            $"Send Progression Event : {progressionStatus} Progression: {progression01} Progression: {progression02} Score: {score}");
    }

    public override void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        string progression03, int score)
    {
        if (!gameAnalyticsInitialized)
            return;

        GAProgressionStatus progressionStatus = (GAProgressionStatus)((int)status);
        GameAnalytics.NewProgressionEvent(progressionStatus, progression01, progression02, progression03, score);
        DebugLog(
            $"Send Progression Event : {progressionStatus} Progression: {progression01} Progression: {progression02} Progression: {progression03} Score: {score}");
    }

    protected override void DebugLog(string _logText)
    {
        Debug.Log("Game Analytics Event: " + _logText);
    }
}
#endif
