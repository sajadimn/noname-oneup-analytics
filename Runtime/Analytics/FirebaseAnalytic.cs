#if ONEUP_FIREBASE
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Firebase.Analytics;

/*
 * An Event is an important occurrence in your app that you want to measure.
 * You can report up to 500 different types of Events per app and you can associate up to 25 unique parameters with each Event type.
 * Some common events are suggested below, but you may also choose to specify custom Event types that are associated with your specific app.
 * Each event type is identified by a unique name. Event names can be up to 40 characters long,
 * may only contain alphanumeric characters and underscores ("_"), and must start with an alphabetic character.
 * The "firebase_", "google_" and "ga_" prefixes are reserved and should not be used.
 */

public class FirebaseAnalytic : BaseAnalytic
{
    protected DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool firebaseInitialized = false;
    private FireBaseAnalyticsConfig fireBaseAnalyticsConfig;
    private string firebaseAppInstanceId;

    public override void Init(BaseAnalyticsConfig config)
    {
        fireBaseAnalyticsConfig = config as FireBaseAnalyticsConfig;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Initialize();
            }
            else
            {
                Debug.LogError(
                    "Could Not Resolve All Firebase Dependencies: " + dependencyStatus);
            }
        });
    }

    protected override void Initialize()
    {
        DebugLog("Enabling FirebaseAnalytics Data Collection");

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
        AnalyticsManager.NotifyFirebaseInitialized();
        FirebaseAnalytics.GetAnalyticsInstanceIdAsync().ContinueWith(task =>
        {
            DebugLog(string.Format("FirebaseAnalytic - Initialize - FirebaseAnalyticsInstanceID: {0}",
                task.Result));
            firebaseAppInstanceId = task.Result;
        });
    }

    public override void SendAnalyticEvent(string[] _name)
    {
        if (!firebaseInitialized)
            return;
        string eventName = "";
        foreach (var key in _name)
        {
            eventName += key + "_";
        }

        eventName = eventName.Remove(eventName.Length - 1);
        FirebaseAnalytics.LogEvent(eventName);
        DebugLog(string.Format("Send Design Event : {0}", eventName));
    }

    public override void SendAnalyticEvent(string[] _name, float _parameterValue)
    {
        if (!firebaseInitialized)
            return;
        string eventName = "";
        foreach (var key in _name)
        {
            eventName += key + "_";
        }

        eventName = eventName.Remove(eventName.Length - 1);
        FirebaseAnalytics.LogEvent(eventName, "_", (int)_parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1}", eventName,
            _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, JObject _parameters)
    {
        if (!firebaseInitialized)
            return;

        int index = 0;
        Parameter[] parameters = new Parameter[_parameters.Count];
        foreach (JProperty property in _parameters.Properties())
        {
            parameters[index] = new Parameter(property.Name, property.Value.ToString());
            index++;
        }

        List<Parameter> logEvent = parameters.ToList();
        FirebaseAnalytics.LogEvent(_name, logEvent.ToArray());
        DebugLog(string.Format("Send Design Event : {0} : {1}", _name, _parameters));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, string _parameterValue)
    {
        if (!firebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent(_name, _parameterName, _parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName,
            _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, double _parameterValue)
    {
        if (!firebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent(_name, _parameterName, _parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName,
            _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, long _parameterValue)
    {
        if (!firebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent(_name, _parameterName, _parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName,
            _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, int _parameterValue)
    {
        if (!firebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent(_name, _parameterName, _parameterValue);
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName,
            _parameterValue));
    }

    public override void SendAnalyticEvent(string _name)
    {
        if (!firebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent(_name);
        DebugLog(string.Format("Send Design Event : {0}", _name));
    }

    public override void SendAnalyticBusinessEvent(string _marketType, string _itemType, string _itemId, int _price,
        string _currency)
    {
        if (!firebaseInitialized)
            return;

        Parameter[] parameters = new Parameter[5];
        int index = 0;
        parameters[index] = new Parameter("MarketType", _marketType);
        index++;
        parameters[index] = new Parameter("ItemType", _itemType);
        index++;
        parameters[index] = new Parameter("ItemId", _itemId);
        index++;
        parameters[index] = new Parameter("Price", _price);
        index++;
        parameters[index] = new Parameter("Currency", _currency);
        List<Parameter> businessEvent = parameters.ToList();
        FirebaseAnalytics.LogEvent("BusinessEvent", businessEvent.ToArray());
        DebugLog(string.Format("Send Business Event : {0} : {1} : {2} : {3} : {4}", _marketType, _itemType, _itemId,
            _price, _currency));
    }

    public override void SendProgressionEvent(ProgressionStatus status, string[] _name, int score)
    {
        if (!firebaseInitialized)
            return;

        string[] result = new string[_name.Length + 2];
        int index = 0;
        foreach (var item in _name)
        {
            result[index] = item;
            index++;
        }

        result[index] = status.ToString();
        index++;
        result[index] = score.ToString();
        SendAnalyticEvent(result);
    }

    public override void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType,
        string itemId)
    {
        if (!firebaseInitialized)
            return;

        string[] reslut = new string[]
            { status.ToString(), currency, amount.ToString(), itemType, itemId };
        SendAnalyticEvent(reslut);
    }

    public override void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType,
        string itemId, IDictionary<string, object> customFields, bool mergeFields = false)
    {
        if (!firebaseInitialized)
            return;

        string[] reslut = new string[]
            { status.ToString(), currency, amount.ToString(), itemType, itemId };
        SendAnalyticEvent(reslut);
    }

    public override void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        int score)
    {
        if (!firebaseInitialized)
            return;

        string[] result = new string[] { $"{status}:{progression01}:{progression02}:{score}" };
        SendAnalyticEvent(result);
    }

    public override void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        string progression03, int score)
    {
        if (!firebaseInitialized)
            return;

        string[] result = new string[] { $"{status}:{progression01}:{progression02}:{progression03}:{score}" };
        SendAnalyticEvent(result);
    }

    public override void ResetAnalyticsData()
    {
        // Reset analytics data for this app instance.
        if (!firebaseInitialized)
            return;
        DebugLog("Reset Analytics Data.");
        FirebaseAnalytics.ResetAnalyticsData();
    }

    public override Task<string> DisplayAnalyticsInstanceId()
    {
        // Get the current app instance ID.
        return FirebaseAnalytics.GetAnalyticsInstanceIdAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                DebugLog("App Instance ID Fetch Was Canceled.");
            }
            else if (task.IsFaulted)
            {
                DebugLog(String.Format("Encounted An Error Fetching App Instance ID {0}",
                    task.Exception.ToString()));
            }
            else if (task.IsCompleted)
            {
                DebugLog(String.Format("App Instance ID: {0}", task.Result));
            }

            return task;
        }).Unwrap();
    }

    public string GetAppInstanceId()
    {
        if (!firebaseInitialized)
            return string.Empty;

        return firebaseAppInstanceId;
    }

    protected override void DebugLog(string _logText)
    {
        Debug.Log("Firebase Event: " + _logText);
    }
}
#endif
