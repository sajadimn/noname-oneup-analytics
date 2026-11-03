using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public abstract class BaseAnalytic
{
    public abstract void Init(BaseAnalyticsConfig config);
    protected abstract void Initialize();
    public abstract void SendAnalyticEvent(string[] _name);
    public abstract void SendAnalyticEvent(string[] _name, float _parameterValue);
    public abstract void SendAnalyticEvent(string _name, JObject _parameters);
    public abstract void SendAnalyticEvent(string _name, string _parameterName, string _parameterValue);
    public abstract void SendAnalyticEvent(string _name, string _parameterName, double _parameterValue);
    public abstract void SendAnalyticEvent(string _name, string _parameterName, long _parameterValue);
    public abstract void SendAnalyticEvent(string _name, string _parameterName, int _parameterValue);
    public abstract void SendAnalyticEvent(string _name);
    public abstract void SendAnalyticBusinessEvent(string _marketType, string _itemType,string _itemId,int _price,string _currency);
    public abstract void SendProgressionEvent(ProgressionStatus status, string[] _name, int score);
    public abstract void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02, int score);
    public abstract void SendProgressionEvent(ProgressionStatus status, string progression01, string progression02, string progression03, int score);
    public abstract void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType, string itemId);
    public abstract void SendResourceEvent(ResourceFlowType status, string currency, float amount, string itemType, string itemId, IDictionary<string, object> customFields, bool mergeFields = false);
    public virtual void ResetAnalyticsData()
    {
    }

    public virtual Task<string> DisplayAnalyticsInstanceId()
    {
        return null;
    }

    protected abstract void DebugLog(string _logText);
}