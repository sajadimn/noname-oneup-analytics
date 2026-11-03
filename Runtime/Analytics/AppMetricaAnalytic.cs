#if ONEUP_APPMETRICA
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Io.AppMetrica;
using Io.AppMetrica.Ecommerce;
using UnityEngine;

public class AppMetricaAnalytic : BaseAnalytic
{
    protected bool appMetricaAnalyticInitialized = false;
    private AppMetricaAnalyticsConfig appMetricaAnalyticsConfig;
    private string appMetricaDeviceIdHash;

    public override void Init(BaseAnalyticsConfig config)
    {
        appMetricaAnalyticsConfig = config as AppMetricaAnalyticsConfig;
        Initialize();
    }

    protected override void Initialize()
    {
        DebugLog("Enabling AppMetrica Data Collection");
        AppMetrica.Activate(new AppMetricaConfig(appMetricaAnalyticsConfig.apiKey)
        {
            Logs = true,
            LocationTracking = true,
            SessionTimeout = 15,
        });
        GetDeviceId();
        appMetricaAnalyticInitialized = true;
    }


    public override void SendAnalyticEvent(string[] _name)
    {
        if (!appMetricaAnalyticInitialized)
            return;
        string eventName = "";
        foreach (var key in _name)
        {
            eventName += key + "_";
        }

        eventName = eventName.Remove(eventName.Length - 1);
        AppMetrica.ReportEvent(eventName);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0}", eventName));
    }

    public override void SendAnalyticEvent(string[] _name, float _parameterValue)
    {
        if (!appMetricaAnalyticInitialized)
            return;
        string eventName = "";
        for (int i = 0; i < _name.Length - 1; i++)
        {
            eventName += _name[i] + "_";
        }

        eventName = eventName.Remove(eventName.Length - 1);
        string eventParameter = $"{{\"{_name[^1]}\":\"{_parameterValue.ToString(CultureInfo.InvariantCulture)}\"}}";
        AppMetrica.ReportEvent(eventName, eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0} : {1}", eventName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, JObject _parameters)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter = _parameters.ToString();
        AppMetrica.ReportEvent(_name, eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0} : {1}", _name, _parameters));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, string _parameterValue)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter = $"{{\"{_parameterName}\":\"{_parameterValue}\"}}";
        AppMetrica.ReportEvent(_name, eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, double _parameterValue)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter =
            $"{{\"{_parameterName}\":\"{_parameterValue.ToString(CultureInfo.InvariantCulture)}\"}}";
        AppMetrica.ReportEvent(_name, eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, long _parameterValue)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter =
            $"{{\"{_parameterName}\":\"{_parameterValue.ToString(CultureInfo.InvariantCulture)}\"}}";
        AppMetrica.ReportEvent(_name, eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name, string _parameterName, int _parameterValue)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter =
            $"{{\"{_parameterName}\":\"{_parameterValue.ToString(CultureInfo.InvariantCulture)}\"}}";
        AppMetrica.ReportEvent(_name, eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0} : {1} : {2}", _name, _parameterName, _parameterValue));
    }

    public override void SendAnalyticEvent(string _name)
    {
        if (!appMetricaAnalyticInitialized)
            return;
        AppMetrica.ReportEvent(_name);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Design Event : {0}", _name));
    }

    public override void SendAnalyticBusinessEvent(string _marketType, string _itemType, string _itemId, int _price,
        string _currency)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        ECommerceProduct product = new ECommerceProduct(_itemId);
        ECommercePrice actualPrice = new ECommercePrice(new ECommerceAmount(_price, _currency));
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "ItemType", _itemType },
            { "MarketType", _marketType }
        };
        product.ActualPrice = actualPrice;
        product.Name = _itemType;
        product.CategoriesPath = new string[] { _marketType };
        product.Payload = payload;

        ECommerceCartItem cartItem = new ECommerceCartItem(product, actualPrice, 1.0);

        ECommerceOrder order = new ECommerceOrder(
            identifier: Guid.NewGuid().ToString(),
            cartItems: new List<ECommerceCartItem> { cartItem }
        );
        order.Payload = payload;

        ECommerceEvent purchaseEvent = ECommerceEvent.PurchaseEvent(order);

        AppMetrica.ReportECommerce(purchaseEvent);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Business Event : {0} : {1} : {2} : {3} : {4}", _marketType, _itemType, _itemId,
            _price, _currency));
        SendRevenue(_marketType, _itemType, _itemId, _price, _currency);
    }

    public void SendRevenue(string _marketType, string _itemType, string _itemId, int _price,
        string _currency)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        Revenue revenue = new Revenue(_price, _currency);
        revenue.ProductID = _itemId;
        revenue.Quantity = 1;
        revenue.Payload = $"{{\"ItemType\":\"{_itemType}\",\"MarketType\":\"{_marketType}\"}}";

        AppMetrica.ReportRevenue(revenue);
        AppMetrica.SendEventsBuffer();
        DebugLog(string.Format("Send Revenue Event : {0} : {1} : {2} : {3} : {4}", _marketType, _itemType, _itemId,
            _price, _currency));
    }

    public override void SendResourceEvent(ResourceFlowType status, string currency, float amount,
        string itemType,
        string itemId)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter =
            $"{{\"Status\":\"{status.ToString()}\",\"ItemType\":\"{itemType}\",\"ItemId\":\"{itemId}\",\"Amount\":\"{amount.ToString(CultureInfo.InvariantCulture)}\",\"Currency\":\"{currency}\"}}";
        AppMetrica.ReportEvent("ResourceFlow", eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(
            $"Send Resource Event : {status} Currency: {currency} Amount: {amount} ItemType: {itemType} ItemId: {itemId}");
    }

    public override void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType,
        string itemId, IDictionary<string, object> customFields, bool mergeFields = false)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter =
            $"{{\"Status\":\"{status.ToString()}\",\"ItemType\":\"{itemType}\",\"ItemId\":\"{itemId}\",\"Amount\":\"{amount.ToString(CultureInfo.InvariantCulture)}\",\"Currency\":\"{currency}\"}}";
        AppMetrica.ReportEvent("ResourceFlow", eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(
            $"Send Resource Event : {status} Currency: {currency} Amount: {amount} ItemType: {itemType} ItemId: {itemId}");
    }

    public override void SendProgressionEvent(ProgressionStatus status, string[] _name, int score)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventName = "";
        foreach (var key in _name)
            eventName += key + "_";
        eventName = eventName.Remove(eventName.Length - 1);
        string eventParameter =
            $"{{\"{eventName}\":\"{score.ToString(CultureInfo.InvariantCulture)}\"}}";
        AppMetrica.ReportEvent(status.ToString(), eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog($"Send Progression Event : {status} EventName: {eventName} Score: {score}");
    }

    public override void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        int score)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter =
            $"{{\"Progression1\":\"{progression01}\",\"Progression2\":\"{progression02}\",\"Score\":\"{score.ToString(CultureInfo.InvariantCulture)}\"}}";
        AppMetrica.ReportEvent(status.ToString(), eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(
            $"Send Progression Event : {status} Progression: {progression01} Progression: {progression02} Score: {score}");
    }

    public override void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02,
        string progression03, int score)
    {
        if (!appMetricaAnalyticInitialized)
            return;

        string eventParameter =
            $"{{\"Progression1\":\"{progression01}\",\"Progression2\":\"{progression02}\",\"Progression3\":\"{progression03}\",\"Score\":\"{score.ToString(CultureInfo.InvariantCulture)}\"}}";
        AppMetrica.ReportEvent(status.ToString(), eventParameter);
        AppMetrica.SendEventsBuffer();
        DebugLog(
            $"Send Progression Event : {status} Progression: {progression01} Progression: {progression02} Progression: {progression03} Score: {score}");
    }

    public void GetDeviceId()
    {
        var keys = new string[]
        {
            StartupParamsKey.AppMetricaUuid,
            StartupParamsKey.AppMetricaDeviceID,
            StartupParamsKey.AppMetricaDeviceIDHash
        };

        AppMetrica.RequestStartupParams(OnDeviceId, keys);
    }

    private void OnDeviceId(StartupParamsResult result, StartupParamsErrorReason errorReason)
    {
        if (result == null)
        {
            DebugLog($"OnDeviceId - Error: {errorReason}");
            return;
        }

        DebugLog(
            $"OnDeviceId - Success \n UUID: {result.Uuid}\n DeviceID: {result.DeviceId}\n DeviceIDHash: {result.DeviceIdHash}");
        appMetricaDeviceIdHash = result.DeviceIdHash;
    }

    public string GetAppMetricaDeviceIdHash()
    {
        if (!appMetricaAnalyticInitialized)
            return string.Empty;

        return appMetricaDeviceIdHash;
    }

    protected override void DebugLog(string _logText)
    {
        Debug.Log("AppMetrica Analytics Event: " + _logText);
    }
}
#endif
